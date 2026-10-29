using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CustomerDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminCustomerManager : IAdminCustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly CustomerBusinessRules _customerBusinessRules;

        public AdminCustomerManager(ICustomerRepository customerRepository,IMapper mapper,CustomerBusinessRules customerBusinessRules)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _customerBusinessRules = customerBusinessRules;
        }

        public async Task<IDataResult<List<CustomerResultDto>>> GetAllAsync()
        {
            var customers = await _customerRepository.GetCustomersWithDetailsAsync();
            var sortedCustomers = customers.OrderByDescending(x => x.CreatedDate).ToList();
            var customerDtos = _mapper.Map<List<CustomerResultDto>>(sortedCustomers);
            return new SuccessDataResult<List<CustomerResultDto>>(customerDtos, "Müşteriler başarıyla listelendi.");
        }

        public async Task<IDataResult<CustomerDetailDto>> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerWithDetailsAsync(id);
            if (customer == null)
            {
                throw new BusinessException("Müşteri bulunamadı.");
            }

            var customerDto = _mapper.Map<CustomerDetailDto>(customer);
            return new SuccessDataResult<CustomerDetailDto>(customerDto, "Müşteri başarıyla getirildi.");
        }

        public async Task<IDataResult<int>> AddForAdminAsync(CustomerCreateByAdminDto customerAddByAdminDto)
        {
            customerAddByAdminDto.NationalIdentity = customerAddByAdminDto.NationalIdentity.Trim();
            IResult? result = BusinessRules.Run(
                await _customerBusinessRules.CheckIfUserExistsAsync(customerAddByAdminDto.UserId),
                await _customerBusinessRules.CheckIfUserAlreadyHasCustomerProfileAsync(customerAddByAdminDto.UserId),
                await _customerBusinessRules.CheckIfNationalIdentityExists(customerAddByAdminDto.NationalIdentity)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var customer = _mapper.Map<Customer>(customerAddByAdminDto);
            await _customerRepository.AddAsync(customer);
            return new SuccessDataResult<int>(customer.Id, "Müşteri profili başarıyla oluşturuldu.");
        }

        public async Task<IResult> UpdateAsync(CustomerUpdateByAdminDto customerUpdateDto)
        {
            customerUpdateDto.NationalIdentity = customerUpdateDto.NationalIdentity.Trim();
            var existingCustomer = await _customerRepository.GetAsync(x => x.Id == customerUpdateDto.Id);
            if (existingCustomer == null)
            {
                throw new BusinessException("Güncellenecek müşteri bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _customerBusinessRules.CheckIfNationalIdentityExistsForUpdate(customerUpdateDto.NationalIdentity, customerUpdateDto.Id)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // Doğru kullanım: Map(Kaynak, Hedef)
            _mapper.Map(customerUpdateDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingCustomer = await _customerRepository.GetAsync(x => x.Id == id);
            if (existingCustomer == null)
            {
                throw new BusinessException("Silinecek müşteri bulunamadı.");
            }

            await _customerRepository.DeleteAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla silindi.");
        }
    }
}
