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
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly CustomerBusinessRules _customerBusinessRules;

        public CustomerManager(ICustomerRepository customerRepository, IMapper mapper, CustomerBusinessRules customerBusinessRules)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _customerBusinessRules = customerBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(int userId, CustomerCreateDto customerAddDto)
        {
            customerAddDto.NationalIdentity = customerAddDto.NationalIdentity.Trim();

            IResult? result = BusinessRules.Run(
                await _customerBusinessRules.CheckIfUserExistsAsync(userId),
                await _customerBusinessRules.CheckIfUserAlreadyHasCustomerProfileAsync(userId),
                await _customerBusinessRules.CheckIfNationalIdentityExists(customerAddDto.NationalIdentity)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var customer = _mapper.Map<Customer>(customerAddDto);
            customer.UserId = userId;
            await _customerRepository.AddAsync(customer);
            return new SuccessDataResult<int>(customer.Id, "Müşteri başarıyla eklendi.");
        }

        public async Task<IDataResult<CustomerDetailDto>> GetMyCustomerProfileAsync(int userId)
        {
            var customer = await _customerRepository.GetCustomerByUserIdWithDetailsAsync(userId);
            if (customer == null)
            {
                throw new BusinessException("Müşteri bilgileri getirelemedi!");
            }

            var customerDto = _mapper.Map<CustomerDetailDto>(customer);
            return new SuccessDataResult<CustomerDetailDto>(customerDto, "Müşteri bilgileri başarıyla getirildi.");
        }

        public async Task<IResult> UpdateMyProfileAsync(int userId, CustomerUpdateMyProfileDto customerUpdateMyProfileDto)
        {
            customerUpdateMyProfileDto.NationalIdentity = customerUpdateMyProfileDto.NationalIdentity.Trim();
            var existingCustomer = await _customerRepository.GetAsync(x => x.UserId == userId);
            if (existingCustomer == null)
            {
                throw new BusinessException("Güncellenecek müşteri bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _customerBusinessRules.CheckIfNationalIdentityExistsForUpdate(customerUpdateMyProfileDto.NationalIdentity, existingCustomer.Id)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(customerUpdateMyProfileDto, existingCustomer);
            await _customerRepository.UpdateAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla güncellendi.");
        }

        public async Task<IResult> CheckIfCustomerExistsByIdAsync(int customerId)
        {
            return await _customerBusinessRules.CheckIfCustomerExistsByIdAsync(customerId);
        }
    }

}
