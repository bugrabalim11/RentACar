using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.Business.Concrete
{
    public class OfficeManager : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public OfficeManager(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<OfficeResultDto>>> GetAllAsync()
        {
            var offices = await _officeRepository.GetAllAsync();
            var sortedOffices = offices.OrderByDescending(x => x.CreatedDate).ToList();
            var officeDtos = _mapper.Map<List<OfficeResultDto>>(sortedOffices);
            return new SuccessDataResult<List<OfficeResultDto>>(officeDtos, "Ofisler başarıyla listelendi.");
        }

        public async Task<IDataResult<OfficeResultDto>> GetByIdAsync(int id)
        {
            var office = await _officeRepository.GetAsync(x => x.Id == id);
            if (office == null)
            {
                throw new BusinessException("Ofis bulunamadı.");
            }

            var officeDto = _mapper.Map<OfficeResultDto>(office);
            return new SuccessDataResult<OfficeResultDto>(officeDto, "Ofis başarıyla getirildi.");
        }
    }
}
