using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ContactInfoDtos;

namespace RentACar.Business.Concrete
{
    public class ContactInfoManager : IContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;

        public ContactInfoManager(IContactInfoRepository contactInfoRepository, IMapper mapper)
        {
            _contactInfoRepository = contactInfoRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<ContactInfoResultDto>>> GetAllAsync()
        {
            var contactInfos = await _contactInfoRepository.GetAllAsync();
            var sortedContactInfos = contactInfos.OrderByDescending(x => x.CreatedDate).ToList();
            var contactInfoDtos = _mapper.Map<List<ContactInfoResultDto>>(sortedContactInfos);
            return new SuccessDataResult<List<ContactInfoResultDto>>(contactInfoDtos, "İletişim bilgileri başarıyla listelendi.");
        }

        public async Task<IDataResult<ContactInfoResultDto>> GetByIdAsync(int id)
        {
            var contactInfo = await _contactInfoRepository.GetAsync(x => x.Id == id);
            if (contactInfo == null)
            {
                throw new BusinessException("İletişim bilgisi bulunamadı.");
            }
            var contactInfoDto = _mapper.Map<ContactInfoResultDto>(contactInfo);
            return new SuccessDataResult<ContactInfoResultDto>(contactInfoDto, "İletişim bilgisi başarıyla getirildi.");
        }
    }
}