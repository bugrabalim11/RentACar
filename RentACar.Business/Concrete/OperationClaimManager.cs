using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OperationClaimAddDto> _addValidator;
        private readonly IValidator<OperationClaimUpdateDto> _updateValidator;

        public OperationClaimManager(IOperationClaimRepository operationClaimRepository, IMapper mapper, IValidator<OperationClaimAddDto> addValidator, IValidator<OperationClaimUpdateDto> updateValidator)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(OperationClaimAddDto operationClaimAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(operationClaimAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var operationClaim = _mapper.Map<OperationClaim>(operationClaimAddDto);
            await _operationClaimRepository.AddAsync(operationClaim);
            return new SuccessResult("Yeni statü başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (existOperationClaim == null)
            {
                return new ErrorResult("Silinecek statü bulunamadı.");
            }

            await _operationClaimRepository.DeleteAsync(existOperationClaim);
            return new SuccessResult("Statü başarıyla silindi.");
        }

        public async Task<IDataResult<List<OperationClaimListDto>>> GetAllAsync()
        {
            var operationClaims = await _operationClaimRepository.GetAllAsync();
            var operationClaimDtos = _mapper.Map<List<OperationClaimListDto>>(operationClaims);
            return new SuccessDataResult<List<OperationClaimListDto>>(operationClaimDtos, "Statüler başarıyla listelendi.");
        }

        public async Task<IDataResult<OperationClaimListDto>> GetByIdAsync(int id)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (operationClaim == null)
            {
                return new ErrorDataResult<OperationClaimListDto>("Statü bulunamadı.");
            }

            var operationClaimDto = _mapper.Map<OperationClaimListDto>(operationClaim);
            return new SuccessDataResult<OperationClaimListDto>(operationClaimDto, "Statü başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(operationClaimUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == operationClaimUpdateDto.Id);
            if (existingOperationClaim == null)
            {
                return new ErrorResult("Güncellenecek statü bulunamadı.");
            }

            _mapper.Map(operationClaimUpdateDto, existingOperationClaim);
            await _operationClaimRepository.UpdateAsync(existingOperationClaim);
            return new SuccessResult("Statü başarıyla güncellendi.");
        }
    }
}
