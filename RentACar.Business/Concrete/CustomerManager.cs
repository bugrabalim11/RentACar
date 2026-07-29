using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
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

        public async Task<IResult> AddAsync(CustomerAddDto customerAddDto)
        {
            await CheckIfUserExistsAsync(customerAddDto.UserId);
            await CheckAlreadyExistCustomer(customerAddDto.UserId);

            var customer = _mapper.Map<Customer>(customerAddDto);
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

            existingCustomer.Status = false;
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

        public async Task<IResult> UpdateAsync(CustomerUpdateDto customerUpdateDto)
        {
            var existingCustomer = await _customerRepository.GetAsync(x => x.Id == customerUpdateDto.Id);
            if (existingCustomer == null)
            {
                return new ErrorResult("Güncellenecek müşteri bulunamadı.");
            }

            // Doğru kullanım: Map(Kaynak, Hedef)
            _mapper.Map(customerUpdateDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla güncellendi.");
        }

        private async Task CheckIfUserExistsAsync(int UserId)
        {
            var existingUser = await _userService.CheckIfUserExistsAsync(UserId);
            if (!existingUser.Success)
            {
                throw new BusinessException(existingUser.Message ?? "Bilinmeyen bir kullanıcı hatası oluştu!");
            }
        }

        private async Task CheckAlreadyExistCustomer(int UserId)
        {
            bool existingCustomer = await _customerRepository.AnyAsync(x => x.UserId == UserId);
            if (existingCustomer)
            {
                throw new BusinessException("Bu kullanıcı sistemde müşteri olarak kayıtlı. Lütfen başka kullancı giriniz!");
            }
        }
    }
}
