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
    public class AdminRentalManager : IAdminRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IAdminCustomerService _adminCustomerService;
        private readonly ICarService _carService;
        private readonly IPosService _posService;
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;
        private readonly IFindexScoreService _findexScoreService;
        private readonly RentalBusinessRules _rentalBusinessRules;

        public AdminRentalManager(IRentalRepository rentalRepository, IMapper mapper, IAdminCustomerService adminCustomerService, ICarService carService, IPosService posService, IPaymentService paymentService, IMailService mailService, RentalBusinessRules rentalBusinessRules, IFindexScoreService findexScoreService)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _adminCustomerService = adminCustomerService;
            _carService = carService;
            _posService = posService;
            _paymentService = paymentService;
            _mailService = mailService;
            _rentalBusinessRules = rentalBusinessRules;
            _findexScoreService = findexScoreService;
        }

        [TransactionScopeAspect]
        public async Task<IDataResult<int>> AddByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto)
        {
            rentalAddByAdminDto.RentDate = rentalAddByAdminDto.RentDate.ToUniversalTime();
            if (rentalAddByAdminDto.ReturnDate.HasValue)
            {
                rentalAddByAdminDto.ReturnDate = rentalAddByAdminDto.ReturnDate.Value.ToUniversalTime();
            }

            var car = await _carService.GetByIdAsync(rentalAddByAdminDto.CarId);
            var customer = await _adminCustomerService.GetByIdAsync(rentalAddByAdminDto.CustomerId);
            int customerFindexScore = _findexScoreService.GetScoreByCustomerId(customer.Data.Id);

            IResult? result = BusinessRules.Run(
                _rentalBusinessRules.CheckIfRentDateBeforeToday(rentalAddByAdminDto.RentDate),
                _rentalBusinessRules.CheckIfCustomerDrivingExperienceIsSufficient(car.Data.MinDrivingExperience, customer.Data.DrivingLicenseYear),
                _rentalBusinessRules.CheckIfCustomerFindexScoreIsSufficient(car.Data.MinFindexScore, customerFindexScore),
                await _rentalBusinessRules.CheckIfCustomerExistsAsync(rentalAddByAdminDto.CustomerId),
                await _rentalBusinessRules.CheckIfCarExistsAsync(rentalAddByAdminDto.CarId),
                await _rentalBusinessRules.CheckIfCarIsInMaintenanceAsync(rentalAddByAdminDto.CarId, rentalAddByAdminDto.RentDate, rentalAddByAdminDto.ReturnDate),
                await _rentalBusinessRules.CheckIfCarAvailable(rentalAddByAdminDto.CarId, rentalAddByAdminDto.RentDate, rentalAddByAdminDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var rental = _mapper.Map<Rental>(rentalAddByAdminDto);

            decimal totalAmount = CalculateTotalAmount(rental.RentDate, rental.ReturnDate, car.Data.DailyPrice);
            rental.TotalAmount = totalAmount;

            var posResult = await _posService.PayAsync(rentalAddByAdminDto.CreditCardInformation, totalAmount);
            if (!posResult.Success)
            {
                throw new BusinessException(posResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
            }

            await _rentalRepository.AddAsync(rental);
            var paymentDto = new PaymentCreateDto
            {
                RentalId = rental.Id,
                Amount = totalAmount,
                TransactionId = Guid.NewGuid().ToString(),
                IsSuccessful = true
            };
            await _paymentService.AddAsync(paymentDto);

            await SendRentalSuccessEmailAsync(
                customer.Data,
                car.Data,
                totalAmount,
                rental.RentDate,
                rental.ReturnDate
                );

            return new SuccessDataResult<int>(rental.Id, "Araç kiralama başarıyla oluşturuldu.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> DeleteAsync(int id)
        {
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == id);
            if (existingRental == null)
            {
                throw new BusinessException("Silinecek araç kiralama bulunamadı.");
            }

            await _rentalRepository.DeleteAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla silindi.");
        }

        public async Task<IDataResult<List<RentalResultDto>>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetRentalsWithDetailsAsync();
            var sortedORentals = rentals.OrderByDescending(x => x.CreatedDate).ToList();
            var rentalsListDtos = _mapper.Map<List<RentalResultDto>>(sortedORentals);
            return new SuccessDataResult<List<RentalResultDto>>(rentalsListDtos, "Kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<RentalDetailDto>> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(id);
            if (rental == null)
            {
                throw new BusinessException("Aranan araç kiralama bulunamadı.");
            }

            var rentalDetailDto = _mapper.Map<RentalDetailDto>(rental);
            return new SuccessDataResult<RentalDetailDto>(rentalDetailDto, "Araç kiralama detayı getirildi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> UpdateByAdminAsync(RentalUpdateByAdminDto rentalUpdateDto)
        {
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == rentalUpdateDto.Id);
            if (existingRental == null)
            {
                throw new BusinessException("Güncellenecek araç kiralama bulunamadı.");
            }

            rentalUpdateDto.RentDate = rentalUpdateDto.RentDate.ToUniversalTime();
            if (rentalUpdateDto.ReturnDate.HasValue)
            {
                rentalUpdateDto.ReturnDate = rentalUpdateDto.ReturnDate.Value.ToUniversalTime();
            }

            var car = await _carService.GetByIdAsync(rentalUpdateDto.CarId);
            var customer = await _adminCustomerService.GetByIdAsync(existingRental.CustomerId);
            int customerFindexScore = _findexScoreService.GetScoreByCustomerId(customer.Data.Id);

            IResult? result = BusinessRules.Run(
                _rentalBusinessRules.CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
                _rentalBusinessRules.CheckIfRentDateBeforeToday(rentalUpdateDto.RentDate),
                _rentalBusinessRules.CheckIfCustomerDrivingExperienceIsSufficient(car.Data.MinDrivingExperience, customer.Data.DrivingLicenseYear),
                _rentalBusinessRules.CheckIfCustomerFindexScoreIsSufficient(car.Data.MinFindexScore, customerFindexScore),
                await _rentalBusinessRules.CheckIfCarExistsAsync(rentalUpdateDto.CarId),
                await _rentalBusinessRules.CheckIfCarIsInMaintenanceAsync(rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate),
                await _rentalBusinessRules.CheckIfCarAvailableForUpdate(rentalUpdateDto.Id, rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(rentalUpdateDto, existingRental);

            var newTotalAmount = CalculateTotalAmount(rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate, car.Data.DailyPrice);
            var difference = newTotalAmount - existingRental.TotalAmount;

            if (difference > 0)
            {
                var posResult = await _posService.PayAsync(rentalUpdateDto.CreditCardInformation, difference);
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

            await SendRentalUpdateEmailAsync(
            customer.Data,
            car.Data,
            existingRental.TotalAmount,
            difference > 0 ? difference : 0, // Eğer difference 0'dan büyükse difference'ın kendisini kullan; DEĞİLSE 0 kullan.
            rentalUpdateDto.RentDate,
            rentalUpdateDto.ReturnDate
            );

            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla güncellendi.");
        }

        private decimal CalculateTotalAmount(DateTime rentDate, DateTime? returnDate, decimal dailyPrice)
        {
            decimal totalDays = 1;
            if (returnDate.HasValue)
            {
                var timeSpan = returnDate.Value - rentDate;

                // Math.Ceiling fonksiyonu bize 'double' döner.
                // Ancak para işlemleri (dailyPrice) sıfır hata payı isteyen 'decimal' tipindedir.
                // C# derleyicisi bu iki farklı dünyanın çarpılmasına güvenlik gereği izin vermez.
                // Bu yüzden (decimal) yazdık.
                totalDays = (decimal)Math.Ceiling(timeSpan.TotalDays);

                if (totalDays == 0 || totalDays < 0) { totalDays = 1; }
            }
            var totalAmount = totalDays * dailyPrice;
            return totalAmount;
        }

        private async Task SendRentalSuccessEmailAsync(CustomerDetailDto customer, CarDetailDto car, decimal totalAmount, DateTime rentDate, DateTime? returnDate)
        {
            try
            {
                var mailRequest = MailTemplateHelper.CreateRentalSuccessMail(
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    $"{car.BrandName} {car.ModelName}",
                    totalAmount,
                    rentDate,
                    returnDate);

                await _mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception)
            {

            }
        }

        private async Task SendRentalUpdateEmailAsync(CustomerDetailDto customer, CarDetailDto car, decimal totalAmount, decimal differenceAmount, DateTime rentDate, DateTime? returnDate)
        {
            try
            {
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
