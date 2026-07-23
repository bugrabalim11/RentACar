using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserOperationClaimAddDto> _addValidator;
        private readonly IValidator<UserOperationClaimUpdateDto> _updateValidator;

        public UserOperationClaimManager(IUserOperationClaimRepository userOperationClaimRepository,IMapper mapper,IValidator<UserOperationClaimAddDto> addValidator, IValidator<UserOperationClaimUpdateDto> updateValidator)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _updateValidator = updateValidator;
        }

        public Task<IResult> AddAsync(UserOperationClaimAddDto userOperationClaimAddDto)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<UserOperationClaimListDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<UserOperationClaimListDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
