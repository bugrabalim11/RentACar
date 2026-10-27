using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.OperationClaimDtos;

namespace RentACar.Business.Concrete
{
    public class AdminOperationClaimManager : IAdminOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;
        private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

        public AdminOperationClaimManager(IOperationClaimRepository operationClaimRepository, IMapper mapper, OperationClaimBusinessRules operationClaimBusinessRules)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
            _operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(OperationClaimCreateDto operationClaimAddDto)
        {
            operationClaimAddDto.Name = operationClaimAddDto.Name.Trim();
            IResult? result = BusinessRules.Run(
                await _operationClaimBusinessRules.CheckIfOperationClaimExistsAsync(operationClaimAddDto.Name)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var operationClaim = _mapper.Map<OperationClaim>(operationClaimAddDto);
            await _operationClaimRepository.AddAsync(operationClaim);
            return new SuccessDataResult<int>(operationClaim.Id, "Yeni yetki başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (existingOperationClaim == null)
            {
                throw new BusinessException("Silinecek yetki bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                _operationClaimBusinessRules.CheckIfOperationClaimNameIsAdmin(existingOperationClaim.Name)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            await _operationClaimRepository.DeleteAsync(existingOperationClaim);
            return new SuccessResult("Yetki başarıyla silindi.");
        }

        public async Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto)
        {
            operationClaimUpdateDto.Name = operationClaimUpdateDto.Name.Trim();
            var existingOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == operationClaimUpdateDto.Id);
            if (existingOperationClaim == null)
            {
                throw new BusinessException("Güncellenecek yetki bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _operationClaimBusinessRules.CheckIfOperationClaimExistsForUpdateAsync(operationClaimUpdateDto.Name, operationClaimUpdateDto.Id),
                _operationClaimBusinessRules.CheckIfOperationClaimNameIsAdmin(existingOperationClaim.Name)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(operationClaimUpdateDto, existingOperationClaim);
            await _operationClaimRepository.UpdateAsync(existingOperationClaim);
            return new SuccessResult("Yetki başarıyla güncellendi.");
        }
    }
}
