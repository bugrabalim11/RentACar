using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentACar.Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserOperationClaimAddDto> _addValidator;
        private readonly IValidator<UserOperationClaimUpdateDto> _updateValidator;

        public UserOperationClaimManager(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper, IValidator<UserOperationClaimAddDto> addValidator, IValidator<UserOperationClaimUpdateDto> updateValidator)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(UserOperationClaimAddDto userOperationClaimAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(userOperationClaimAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // İŞ KURALI(BUSINESS RULE) KONTROLÜ - YENİ EKLENEN KISIM
            var logicResult = await CheckIfUserHasThisClaimAlready(userOperationClaimAddDto.UserId, userOperationClaimAddDto.OperationClaimId);

            // Eğer kuralımızdan Success (Başarılı) dönmezse, demek ki adamda bu rütbe zaten var. 
            // İşlemi iptal edip direkt hatayı vezneye yolluyoruz!
            if (!logicResult.Success)
            {
                return logicResult;
            }

            var userOperationClaim = _mapper.Map<UserOperationClaim>(userOperationClaimAddDto);
            await _userOperationClaimRepository.AddAsync(userOperationClaim);
            return new SuccessResult("Kullanıcıya yetki başarıyla atandı.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == id);
            if (existingUserOperationClaim == null)
            {
                return new ErrorResult("Silinmek istenen yetki ataması bulunamadı.");
            }

            await _userOperationClaimRepository.DeleteAsync(existingUserOperationClaim);
            return new SuccessResult("Kullanıcının yetkisi başarıyla kaldırıldı.");
        }

        public async Task<IDataResult<List<UserOperationClaimListDto>>> GetAllAsync()
        {
            var userOperationClaims = await _userOperationClaimRepository.GetAllAsync();
            var userOperationClaimDtos = _mapper.Map<List<UserOperationClaimListDto>>(userOperationClaims);
            return new SuccessDataResult<List<UserOperationClaimListDto>>(userOperationClaimDtos, "Tüm kullanıcı yetkileri listelendi.");
        }

        public async Task<IDataResult<UserOperationClaimListDto>> GetByIdAsync(int id)
        {
            var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == id);
            if (userOperationClaim == null)
            {
                return new ErrorDataResult<UserOperationClaimListDto>("Belirtilen yetki ataması bulunamadı.");
            }

            var userOperationClaimDto = _mapper.Map<UserOperationClaimListDto>(userOperationClaim);
            return new SuccessDataResult<UserOperationClaimListDto>(userOperationClaimDto, "Yetki ataması başarıyla getirildi.");
        }

        public async Task< IDataResult<List<UserOperationClaimDetailDto>>> GetClaimDetailsAsync()
        {
            // 1. Telsizle depocuya (Repository) seslen ve özel tabağı (JOIN sorgusunu) iste
            var claimDetails =await _userOperationClaimRepository.GetClaimDetailsAsync();

            // 2. Gelen bu özel tabağı, şirketimizin resmi "Başarılı Sonuç" kutusuna koy ve mesajını ekle
            return new SuccessDataResult<List<UserOperationClaimDetailDto>>(claimDetails, "Kullanıcı yetkileri detaylı olarak listelendi.");
        }

        public async Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(userOperationClaimUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == userOperationClaimUpdateDto.Id);
            if (existingUserOperationClaim == null)
            {
                return new ErrorResult("Güncellenmek istenen yetki ataması bulunamadı.");
            }

            _mapper.Map(userOperationClaimUpdateDto, existingUserOperationClaim);
            await _userOperationClaimRepository.UpdateAsync(existingUserOperationClaim);
            return new SuccessResult("Kullanıcı yetkisi başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfUserHasThisClaimAlready(int userId, int operationClaimId)
        {
            // Telsizle depoya sor: "Böyle bir kayıt var mı? (Evet/Hayır)"
            // AnyAsync sana true ya da false dönecek.
            var result = await _userOperationClaimRepository.AnyAsync(uoc => uoc.UserId == userId && uoc.OperationClaimId == operationClaimId);

            // result zaten true veya false olduğu için direkt if (result) yazabiliriz.
            // "Eğer result true ise (yani kayıt VARSA)" demek ki adam zaten bu rütbeye sahip!
            if (result)
            {
                return new ErrorResult("Kullanıcı zaten bu yetkiye sahip.");
            }
            return new SuccessResult();
        }
    }
}
