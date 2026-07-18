using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ColorDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CustomerAddDto> _addValidator;
        private readonly IValidator<CustomerUpdateDto> _updateValidator;

        public CustomerManager(ICustomerRepository customerRepository, IMapper mapper, IValidator<CustomerAddDto> addValidator, IValidator<CustomerUpdateDto> updateValidator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(CustomerAddDto customerAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(customerAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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

            await _customerRepository.DeleteAsync(existingCustomer);
            return new SuccessResult("Müşteri başarıyla silindi.");
        }

        public async Task<IDataResult<List<CustomerListDto>>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customerDtos = _mapper.Map<List<CustomerListDto>>(customers);
            return new SuccessDataResult<List<CustomerListDto>>(customerDtos, "Müşteriler başarıyla listelendi.");
        }

        public async Task<IDataResult<CustomerListDto>> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetAsync(x => x.Id == id);
            if (customer == null)
            {
                return new ErrorDataResult<CustomerListDto>("Müşteri bulunamadı.");
            }

            var customerDto = _mapper.Map<CustomerListDto>(customer);
            return new SuccessDataResult<CustomerListDto>(customerDto, "Müşteri başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(CustomerUpdateDto customerUpdateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(customerUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
    }
}
