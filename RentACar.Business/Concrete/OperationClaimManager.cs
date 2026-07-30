using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;

        public OperationClaimManager(IOperationClaimRepository operationClaimRepository, IMapper mapper)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(OperationClaimAddDto operationClaimAddDto)
        {
            operationClaimAddDto.Name = operationClaimAddDto.Name.Trim();
            IResult? result = BusinessRules.Run(await CheckIfOperationClaimExistsAsync(operationClaimAddDto.Name));
            if (result != null)
            {
                return result;
            }

            var operationClaim = _mapper.Map<OperationClaim>(operationClaimAddDto);
            await _operationClaimRepository.AddAsync(operationClaim);
            return new SuccessResult("Yeni yetki başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (existingOperationClaim == null)
            {
                return new ErrorResult("Silinecek yetki bulunamadı.");
            }

            existingOperationClaim.Status = false;
            await _operationClaimRepository.UpdateAsync(existingOperationClaim);
            return new SuccessResult("Yetki başarıyla silindi.");
        }

        public async Task<IDataResult<List<OperationClaimListDto>>> GetAllAsync()
        {
            var operationClaims = await _operationClaimRepository.GetAllAsync();
            var operationClaimDtos = _mapper.Map<List<OperationClaimListDto>>(operationClaims);
            return new SuccessDataResult<List<OperationClaimListDto>>(operationClaimDtos, "Yetkiler başarıyla listelendi.");
        }

        public async Task<IDataResult<OperationClaimListDto>> GetByIdAsync(int id)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (operationClaim == null)
            {
                return new ErrorDataResult<OperationClaimListDto>("Yetki bulunamadı.");
            }

            var operationClaimDto = _mapper.Map<OperationClaimListDto>(operationClaim);
            return new SuccessDataResult<OperationClaimListDto>(operationClaimDto, "Yetki başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto)
        {
            operationClaimUpdateDto.Name = operationClaimUpdateDto.Name.Trim();
            IResult? result = BusinessRules.Run(await CheckIfOperationClaimExistsForUpdateAsync(operationClaimUpdateDto.Name, operationClaimUpdateDto.Id));
            if (result != null)
            {
                return result;
            }

            var existingOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == operationClaimUpdateDto.Id);
            if (existingOperationClaim == null)
            {
                return new ErrorResult("Güncellenecek yetki bulunamadı.");
            }

            _mapper.Map(operationClaimUpdateDto, existingOperationClaim);
            await _operationClaimRepository.UpdateAsync(existingOperationClaim);
            return new SuccessResult("Yetki başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfOperationClaimExistsAsync(string operationClaim)
        {
            // Bekçiye == dediğinde "Bana harfi harfine BMW'yi bul" dersin. Bekçiye ILike dediğinde
            // ise "Bana okunuşu bmw olan adamı bul,
            // harflerin büyük küçük olması umurumda değil" dersin.
            bool existOperationClaim = await _operationClaimRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, operationClaim));
            if (existOperationClaim)
            {
                return new ErrorResult("Bu statü sistemde kayıtlı! Lütfen başka statü girmeyi deneyin.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfOperationClaimExistsForUpdateAsync(string operationClaim, int operationClaimId)
        {
            bool isExist = await _operationClaimRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, operationClaim) && x.Id != operationClaimId);
            if (isExist)
            {
                return new ErrorResult("Bu statü sistemde kayıtlı! Lütfen başka statü girmeyi deneyin.");
            }
            return new SuccessResult();
        }
    }
}