using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarImageDtos;

namespace RentACar.Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        private readonly ICarImageRepository _carImageRepository;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarImageManager(ICarImageRepository carImageRepository, ICarService carService, IMapper mapper)
        {
            _carImageRepository = carImageRepository;
            _carService = carService;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<CarImageDetailDto>>> GetImagesByCarIdAsync(int carId)
        {
            var car = await _carService.GetByIdAsync(carId);
            var carImages = await _carImageRepository.GetImagesWithCarDetailsAsync(carId);

            // EF Core liste dönerken asla null dönmez, boş liste döner. Bu yüzden null yerine Any() ile kutunun içini kontrol ediyoruz.
            if (!carImages.Any())
            {
                return new SuccessDataResult<List<CarImageDetailDto>>(new List<CarImageDetailDto>(), "Resim yok.");
            }

            var sortedCarImages = carImages.OrderByDescending(x => x.CreatedDate).ToList();
            var dtoList = _mapper.Map<List<CarImageDetailDto>>(sortedCarImages);
            return new SuccessDataResult<List<CarImageDetailDto>>(dtoList, "Bu araca ait resimler başarıyla getirildi.");
        }
    }
}