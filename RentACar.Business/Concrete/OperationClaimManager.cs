using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;
using RentACar.Core.Exceptions;
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
            await CheckIfOperationClaimExistsAsync(operationClaimAddDto.Name);

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
            await CheckIfOperationClaimExistsForUpdateAsync(operationClaimUpdateDto.Name, operationClaimUpdateDto.Id);

            var existingOperationClaim = await _operationClaimRepository.GetAsync(x => x.Id == operationClaimUpdateDto.Id);
            if (existingOperationClaim == null)
            {
                return new ErrorResult("Güncellenecek yetki bulunamadı.");
            }

            _mapper.Map(operationClaimUpdateDto, existingOperationClaim);
            await _operationClaimRepository.UpdateAsync(existingOperationClaim);
            return new SuccessResult("Yetki başarıyla güncellendi.");
        }

        private async Task CheckIfOperationClaimExistsAsync(string operationClaim)
        {
            // Bekçiye == dediğinde "Bana harfi harfine BMW'yi bul" dersin. Bekçiye ILike dediğinde
            // ise "Bana okunuşu bmw olan adamı bul,
            // harflerin büyük küçük olması umurumda değil" dersin.
            bool existOperationClaim = await _operationClaimRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, operationClaim));
            if (existOperationClaim)
            {
                throw new BusinessException("Bu statü sistemde kayıtlı! Lütfen başka statü girmeyi deneyin.");
            }
        }

        private async Task CheckIfOperationClaimExistsForUpdateAsync(string operationClaim, int operationClaimId)
        {
            bool isExist = await _operationClaimRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, operationClaim) && x.Id != operationClaimId);
            if (isExist)
            {
                throw new BusinessException("Bu statü sistemde kayıtlı! Lütfen başka statü girmeyi deneyin.");
            }
        }
    }
}