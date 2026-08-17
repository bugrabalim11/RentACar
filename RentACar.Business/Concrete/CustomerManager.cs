using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CustomerDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CustomerManager(ICustomerRepository customerRepository, IMapper mapper, IUserService userService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IResult> AddAsync(int userId, CustomerAddDto customerAddDto)
        {
            customerAddDto.NationalIdentity = customerAddDto.NationalIdentity.Trim();

            IResult? result = BusinessRules.Run(
            await CheckIfUserAlreadyHasCustomerProfileAsync(userId),
            await CheckIfUserExistsAsync(userId),
            await CheckAlreadyExistCustomer(userId),
            await CheckIfNationalIdentityExists(customerAddDto.NationalIdentity)
            );

            if (result != null)
            {
                return result;
            }

            var customer = _mapper.Map<Customer>(customerAddDto);
            customer.UserId = userId;
            await _customerRepository.AddAsync(customer);
            return new SuccessResult("Müşteri başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingCustomer = await _customerRepository.GetAsync(x => x.Id == id);
            if (existingCustomer == null)
            {
                return new ErrorResult("Silinecek müşteri bulunamadı.");
            }

            existingCustomer.IsDeleted = true;
            existingCustomer.DeletedDate = DateTime.UtcNow;
            await _customerRepository.UpdateAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla silindi.");
        }

        public async Task<IDataResult<List<CustomerListDto>>> GetAllAsync()
        {
            var customers = await _customerRepository.GetCustomersWithDetailsAsync();
            var customerDtos = _mapper.Map<List<CustomerListDto>>(customers);
            return new SuccessDataResult<List<CustomerListDto>>(customerDtos, "Müşteriler başarıyla listelendi.");
        }

        public async Task<IDataResult<CustomerDetailDto>> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerWithDetailsAsync(id);
            if (customer == null)
            {
                return new ErrorDataResult<CustomerDetailDto>("Müşteri bulunamadı.");
            }

            var customerDto = _mapper.Map<CustomerDetailDto>(customer);
            return new SuccessDataResult<CustomerDetailDto>(customerDto, "Müşteri başarıyla getirildi.");
        }

        public async Task<IDataResult<CustomerDetailDto>> GetMyCustomerProfileAsync(int userId)
        {
            var customer = await _customerRepository.GetCustomerByUserIdWithDetailsAsync(userId);
            if (customer == null)
            {
                return new ErrorDataResult<CustomerDetailDto>("Müşteri bilgileri getirelemedi!");
            }

            var customerDto = _mapper.Map<CustomerDetailDto>(customer);
            return new SuccessDataResult<CustomerDetailDto>(customerDto, "Müşteri bilgileri başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(CustomerUpdateDto customerUpdateDto)
        {
            customerUpdateDto.NationalIdentity = customerUpdateDto.NationalIdentity.Trim();
            var existingCustomer = await _customerRepository.GetAsync(x => x.Id == customerUpdateDto.Id);
            if (existingCustomer == null)
            {
                return new ErrorResult("Güncellenecek müşteri bulunamadı.");
            }

            IResult? result = BusinessRules.Run(await CheckIfNationalIdentityExistsForUpdate(customerUpdateDto.NationalIdentity, customerUpdateDto.Id));
            if (result != null)
            {
                return result;
            }

            // Doğru kullanım: Map(Kaynak, Hedef)
            _mapper.Map(customerUpdateDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla güncellendi.");
        }

        public async Task<IResult> UpdateMyProfileAsync(int userId, CustomerUpdateMyProfileDto customerUpdateMyProfileDto)
        {
            customerUpdateMyProfileDto.NationalIdentity = customerUpdateMyProfileDto.NationalIdentity.Trim();
            var existingCustomer = await _customerRepository.GetAsync(x => x.UserId == userId);
            if (existingCustomer == null)
            {
                return new ErrorResult("Güncellenecek müşteri bulunamadı.");
            }

            IResult? result = BusinessRules.Run(await CheckIfNationalIdentityExistsForUpdate(customerUpdateMyProfileDto.NationalIdentity, existingCustomer.Id));
            if (result != null)
            {
                return result;
            }

            _mapper.Map(customerUpdateMyProfileDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfUserExistsAsync(int UserId)
        {
            var existingUser = await _userService.CheckIfUserExistsAsync(UserId);
            if (!existingUser.Success)
            {
                return new ErrorResult(existingUser.Message ?? "Bilinmeyen bir kullanıcı hatası oluştu!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckAlreadyExistCustomer(int UserId)
        {
            bool existingCustomer = await _customerRepository.AnyAsync(x => x.UserId == UserId);
            if (existingCustomer)
            {
                return new ErrorResult("Bu kullanıcı sistemde müşteri olarak kayıtlı. Lütfen başka kullancı giriniz!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfNationalIdentityExistsForUpdate(string nationalId, int currentCustomerId)
        {
            bool existingNationalId = await _customerRepository.AnyAsync(x => x.NationalIdentity == nationalId && x.Id != currentCustomerId);
            if (existingNationalId)
            {
                return new ErrorResult("Bu kimlik numarası zaten kayıtlı!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfNationalIdentityExists(string nationalId)
        {
            bool existingNationalId = await _customerRepository.AnyAsync(x => x.NationalIdentity == nationalId);
            if (existingNationalId)
            {
                return new ErrorResult("Bu kimlik numarası zaten kayıtlı! Lütfen tekrar deneyiniz.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfUserAlreadyHasCustomerProfileAsync(int userId)
        {
            bool customerIsExist = await _customerRepository.AnyAsync(x => x.UserId == userId);
            if (customerIsExist)
            {
                return new ErrorResult("Sistemde zaten bir müşteri profiliniz bulunmaktadır!");
            }
            return new SuccessResult();
        }
    }

}
