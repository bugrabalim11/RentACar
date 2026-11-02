using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Helpers;
using RentACar.Business.Rules;
using RentACar.Core.Aspects.Autofac.Transaction;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Mailing;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.PaymentDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IPosService _posService;
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IFindexScoreService _findexScoreService;
        private readonly RentalBusinessRules _rentalBusinessRules;

        public RentalManager(IRentalRepository rentalRepository, IMapper mapper, IPosService posService, IPaymentService paymentService, IMailService mailService, ICarService carService, ICustomerService customerService, RentalBusinessRules rentalBusinessRules, IFindexScoreService findexScoreService)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _posService = posService;
            _paymentService = paymentService;
            _mailService = mailService;
            _carService = carService;
            _customerService = customerService;
            _rentalBusinessRules = rentalBusinessRules;
            _findexScoreService = findexScoreService;
        }

        [TransactionScopeAspect]
        public async Task<IDataResult<int>> AddAsync(RentalCreateDto rentalAddDto, int userId)
        {
            rentalAddDto.RentDate = rentalAddDto.RentDate.ToUniversalTime();
            if (rentalAddDto.ReturnDate.HasValue)
            {
                rentalAddDto.ReturnDate = rentalAddDto.ReturnDate.Value.ToUniversalTime();
            }

            var customerResult = await _customerService.GetMyCustomerProfileAsync(userId);
            if (!customerResult.Success)
            {
                throw new BusinessException("Kiralama yapabilmek için lütfen ilk önce müşteri profilinizi oluşturun!");
            }
            var car = await _carService.GetByIdAsync(rentalAddDto.CarId);
            int customerFindexScore = _findexScoreService.GetScoreByCustomerId(customerResult.Data.Id);

            IResult? result = BusinessRules.Run(
                _rentalBusinessRules.CheckIfRentDateBeforeToday(rentalAddDto.RentDate),
                _rentalBusinessRules.CheckIfCustomerDrivingExperienceIsSufficient(car.Data.MinDrivingExperience, customerResult.Data.DrivingLicenseYear),
                _rentalBusinessRules.CheckIfCustomerFindexScoreIsSufficient(car.Data.MinFindexScore, customerFindexScore),
                await _rentalBusinessRules.CheckIfCarExistsAsync(rentalAddDto.CarId),
                await _rentalBusinessRules.CheckIfCarIsInMaintenanceAsync(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate),
                await _rentalBusinessRules.CheckIfCarAvailable(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var rental = _mapper.Map<Rental>(rentalAddDto);
            rental.CustomerId = customerResult.Data.Id;

            decimal totalAmount = CalculateTotalAmount(rental.RentDate, rental.ReturnDate, car.Data.DailyPrice);
            rental.TotalAmount = totalAmount;

            var posResult = await _posService.PayAsync(rentalAddDto.CreditCardInformation, totalAmount);
            if (!posResult.Success)
            {
                throw new BusinessException(posResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
            }

            await _rentalRepository.AddAsync(rental);

            var paymentDto = new PaymentCreateDto
            {
                RentalId = rental.Id,
                Amount = totalAmount,
                TransactionId = Guid.NewGuid().ToString(), // Şimdilik uydurma banka işlem numarası
                IsSuccessful = true
            };
            await _paymentService.AddAsync(paymentDto);

            await SendRentalSuccessEmailAsync(
                customerResult.Data,
                car.Data,
                totalAmount,
                rental.RentDate,
                rental.ReturnDate
                );

            return new SuccessDataResult<int>(rental.Id, "Araç kiralama başarıyla oluşturuldu.");
        }

        public async Task<IResult> CheckIfOfficeHasRentalsAsync(int officeId)
        {
            return await _rentalBusinessRules.CheckIfOfficeHasRentalsAsync(officeId);
        }

        public async Task<IDataResult<List<RentalResultDto>>> GetAllByUserIdAsync(int userId)
        {
            var rentals = await _rentalRepository.GetRentalsByUserIdAsync(userId);
            if (rentals == null || !rentals.Any())
            {
                throw new BusinessException("Kullanıcıya ait kiralama işlemleri bulunamadı.");
            }

            var sortedRentals = rentals.OrderByDescending(x => x.CreatedDate).ToList();
            var mappedRentals = _mapper.Map<List<RentalResultDto>>(sortedRentals);
            return new SuccessDataResult<List<RentalResultDto>>(mappedRentals, "Kullanıcıya ait kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<RentalDetailDto>> GetMyRentalByIdAsync(int rentalId, int userId)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (rental == null)
            {
                throw new BusinessException("Aradağınız kiralama bulunamadı!");
            }

            if (rental.Customer.UserId != userId)
            {
                throw new BusinessException("Güvenlik İhlali: Bu kiralama kaydını (faturayı) görüntüleme yetkiniz yok!");
            }

            var mappedRental = _mapper.Map<RentalDetailDto>(rental);
            return new SuccessDataResult<RentalDetailDto>(mappedRental, "Kiralama detaylarınız başarıyla getirildi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> UpdateMyRentalAsync(int userId, int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto)
        {
            rentalUpdateReturnDateDto.ReturnDate = rentalUpdateReturnDateDto.ReturnDate.ToUniversalTime();

            var existingRental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (existingRental == null) return new ErrorResult("Kiralama bulunamadı!");

            if (existingRental.Customer.UserId != userId)
            {
                throw new BusinessException("Bu kiralamayı güncellemeye yetkiniz yok!");
            }

            IResult? result = BusinessRules.Run(
                _rentalBusinessRules.CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
                await _rentalBusinessRules.CheckIfCarAvailableForUpdate(rentalId, existingRental.CarId, existingRental.RentDate, rentalUpdateReturnDateDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var newTotalAmount = CalculateTotalAmount(existingRental.RentDate, rentalUpdateReturnDateDto.ReturnDate, existingRental.Car.DailyPrice);
            var difference = newTotalAmount - existingRental.TotalAmount;
            if (difference > 0)
            {
                var posResult = await _posService.PayAsync(rentalUpdateReturnDateDto.CreditCardInformation, difference);
                if (!posResult.Success)
                {
                    throw new BusinessException(posResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
                }

                var paymentDto = new PaymentCreateDto
                {
                    RentalId = existingRental.Id,
                    Amount = difference,
                    TransactionId = Guid.NewGuid().ToString(),
                    IsSuccessful = true
                };
                await _paymentService.AddAsync(paymentDto);

                existingRental.TotalAmount = newTotalAmount;
            }

            var carDto = _mapper.Map<CarDetailDto>(existingRental.Car);
            var customerDto = _mapper.Map<CustomerDetailDto>(existingRental.Customer);

            await SendRentalUpdateEmailAsync(
            customerDto,
            carDto,
            existingRental.TotalAmount,
            difference > 0 ? difference : 0, // Eğer difference 0'dan büyükse difference'ın kendisini kullan; DEĞİLSE 0 kullan.
            existingRental.RentDate,
            rentalUpdateReturnDateDto.ReturnDate
            );

            existingRental.ReturnDate = rentalUpdateReturnDateDto.ReturnDate;
            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç teslim tarihiniz başarıyla güncellendi.");
        }

        private decimal CalculateTotalAmount(DateTime rentDate, DateTime? returnDate, decimal dailyPrice)
        {
            decimal totalDays = 1;
            if (returnDate.HasValue)
            {
                var timeSpan = returnDate.Value - rentDate;

                // SENİOR NOTU: Neden (decimal) ile Casting (Bilinçli Tip Dönüşümü) yaptık?
                // Math.Ceiling fonksiyonu bize 'double' döner (Bilim İnsanı mantığı: virgüllü ve esnek).
                // Ancak para işlemleri (dailyPrice) sıfır hata payı isteyen 'decimal' (Muhasebeci mantığı) tipindedir.
                // C# derleyicisi bu iki farklı dünyanın çarpılmasına güvenlik gereği izin vermez.
                // Bu yüzden (decimal) yazarak "Bilim insanının yuvarladığı sayıyı, muhasebecinin diline çevir" dedik.
                totalDays = (decimal)Math.Ceiling(timeSpan.TotalDays);

                if (totalDays == 0 || totalDays < 0) { totalDays = 1; }
            }
            var totalAmount = totalDays * dailyPrice;
            return totalAmount;
        }

        // Veznedarın fatura ve kargo işlerini halleden özel asistanı
        private async Task SendRentalSuccessEmailAsync(CustomerDetailDto customer, CarDetailDto car, decimal totalAmount, DateTime rentDate, DateTime? returnDate)
        {
            try
            {
                // 1. Matbaacıdan hazır kargo kutusunu (Zarfı) al
                var mailRequest = MailTemplateHelper.CreateRentalSuccessMail(
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    $"{car.BrandName} {car.ModelName}",
                    totalAmount,
                    rentDate,
                    returnDate);

                // 2. Postacıya teslim et ve yola çıkar
                await _mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception)
            {
                // Mail atılamasa bile kiralama işlemi iptal OLMASIN! 
                // Hata yutulur (İstersen buraya ileride NLog ile loglama yapabiliriz).
            }
        }

        private async Task SendRentalUpdateEmailAsync(CustomerDetailDto customer, CarDetailDto car, decimal totalAmount, decimal differenceAmount, DateTime rentDate, DateTime? returnDate)
        {
            try
            {
                // 1. Matbaacıdan hazır kargo kutusunu (Zarfı) al
                var mailRequest = MailTemplateHelper.CreateRentalUpdateMail(
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    $"{car.BrandName} {car.ModelName}",
                    totalAmount,
                    differenceAmount,
                    rentDate,
                    returnDate);

                await _mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception)
            {

            }
        }
    }
}