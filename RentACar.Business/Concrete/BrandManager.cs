using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.BrandDtos;

namespace RentACar.Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinessRules;

        public BrandManager(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<IResult> CheckIfBrandExistsAsync(int brandId)
        {
            return await _brandBusinessRules.CheckIfBrandExistsAsync(brandId);
        }

        public async Task<IDataResult<List<BrandResultDto>>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();

            // SENİOR DOKUNUŞU: UI'da zıplamayı engelle, en son eklenen marka hep en tepede çıksın!
            var sortedBrands = brands.OrderByDescending(x => x.CreatedDate).ToList();

            var brandDtos = _mapper.Map<List<BrandResultDto>>(sortedBrands);
            return new SuccessDataResult<List<BrandResultDto>>(brandDtos, "Markalar başarıyla listelendi.");
        }

        public async Task<IDataResult<BrandResultDto>> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(x => x.Id == id);
            if (brand == null)
            {
                throw new BusinessException("Aranan marka bulunamadı.");
            }

            var brandDto = _mapper.Map<BrandResultDto>(brand);
            return new SuccessDataResult<BrandResultDto>(brandDto, "Marka başarıyla getirildi.");
        }
    }
}