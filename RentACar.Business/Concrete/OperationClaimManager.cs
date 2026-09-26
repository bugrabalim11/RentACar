using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;
using RentACar.Core.Exceptions;
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

        public async Task<IDataResult<int>> AddAsync(OperationClaimCreateDto operationClaimAddDto)
        {
            operationClaimAddDto.Name = operationClaimAddDto.Name.Trim();
            IResult? result = BusinessRules.Run(await CheckIfOperationClaimExistsAsync(operationClaimAddDto.Name));
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

            IResult? result = BusinessRules.Run(CheckIfOperationClaimNameIsAdmin(existingOperationClaim.Name));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            existingOperationClaim.IsDeleted = true;
            existingOperationClaim.DeletedDate = DateTime.UtcNow;
            await _operationClaimRepository.UpdateAsync(existingOperationClaim);
            return new SuccessResult("Yetki başarıyla silindi.");
        }

        public async Task<IDataResult<List<OperationClaimResultDto>>> GetAllAsync()
        {
            var operationClaims = await _operationClaimRepository.GetAllAsync();
            var operationClaimDtos = _mapper.Map<List<OperationClaimResultDto>>(operationClaims);
            return new SuccessDataResult<List<OperationClaimResultDto>>(operationClaimDtos, "Yetkiler başarıyla listelendi.");
        }

        public async Task<IDataResult<OperationClaimResultDto>> GetByIdAsync(int id)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(x => x.Id == id);
            if (operationClaim == null)
            {
                throw new BusinessException("Yetki bulunamadı.");
            }

            var operationClaimDto = _mapper.Map<OperationClaimResultDto>(operationClaim);
            return new SuccessDataResult<OperationClaimResultDto>(operationClaimDto, "Yetki başarıyla getirildi.");
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
            await CheckIfOperationClaimExistsForUpdateAsync(operationClaimUpdateDto.Name, operationClaimUpdateDto.Id),
            CheckIfOperationClaimNameIsAdmin(existingOperationClaim.Name)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
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

        private IResult CheckIfOperationClaimNameIsAdmin(string name)
        {
            // StringComparison.OrdinalIgnoreCase: "Büyük/küçük harfe takılma ve işletim sisteminin diline
            // (Türkçe/İngilizce vb.) bakmadan evrensel karşılaştır" demektir. Hafızada yeni kutu açmaz, %100 performanslıdır!
            if (name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return new ErrorResult("Sistemin temel yetkileri üzerinde değişiklik yapılmasına izin verilmez!");
            }
            return new SuccessResult();
        }
    }
}