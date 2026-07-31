using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Helpers.FileHelper;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarImageDtos;
using RentACar.Entities.Concrete;
using IResult = RentACar.Core.Utilities.Results.IResult;

namespace RentACar.Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        private readonly ICarImageRepository _carImageRepository;
        private readonly IFileHelper _fileHelper;

        public CarImageManager(ICarImageRepository carImageRepository, IFileHelper fileHelper)
        {
            _carImageRepository = carImageRepository;
            _fileHelper = fileHelper;
        }

        public async Task<IResult> AddAsync(CarImageAddDto carImageAddDto)
        {
            string? imagePath = _fileHelper.Upload(carImageAddDto.ImageFile, "wwwroot\\Images");

            if (imagePath == null)
            {
                return new ErrorResult("Resim yüklenirken bir hata oluştu veya dosya boş.");
            }

            CarImage carImage = new CarImage
            {
                CarId = carImageAddDto.CarId,
                ImagePath = imagePath
            };

            await _carImageRepository.AddAsync(carImage);
            return new SuccessResult("Resim başarıyla eklendi.");
        }

        public Task<IResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<CarImageDetailDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<CarImageDetailDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
