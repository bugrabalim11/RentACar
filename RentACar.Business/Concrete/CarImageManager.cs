using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Business;
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
            IResult? result = BusinessRules.Run(await CheckIfCarImageLimitExceededAsync(carImageAddDto.CarId));
            if (result != null)
            {
                return result;
            }
            string? imagePath = _fileHelper.Upload(carImageAddDto.ImageFile, "wwwroot\\Images");

            if (imagePath == null)
            {
                return new ErrorResult("Resim yüklenirken bir hata oluştu veya dosya boş.");
            }

            CarImage carImage = new CarImage
            {
                CarId = carImageAddDto.CarId,
                ImagePath = imagePath,
                UploadDate = DateTime.UtcNow
            };

            await _carImageRepository.AddAsync(carImage);
            return new SuccessResult("Resim başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var result = await _carImageRepository.GetAsync(x => x.Id == id);
            if (result == null)
            {
                return new ErrorResult("Resim bulunamadı!");
            }

            _fileHelper.Delete(result.ImagePath);
            await _carImageRepository.DeleteAsync(result);
            return new SuccessResult("Resim başarıyla silindi.");
        }

        public Task<IDataResult<List<CarImageDetailDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<CarImageDetailDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto)
        {
            var existingCarImage = await _carImageRepository.GetAsync(x => x.Id == carImageUpdateDto.Id);
            if (existingCarImage == null)
            {
                return new ErrorResult("Resim bulunamadı!");
            }

            string? newImagePath = _fileHelper.Update(carImageUpdateDto.ImageFile, existingCarImage.ImagePath, "wwwroot\\Images");
            if (newImagePath == null)
            {
                return new ErrorResult("Resim güncellenirken bir hata oluştu veya dosya boş.");
            }

            existingCarImage.ImagePath = newImagePath;
            existingCarImage.UploadDate = DateTime.UtcNow;

            await _carImageRepository.UpdateAsync(existingCarImage);
            return new SuccessResult("Resim başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfCarImageLimitExceededAsync(int carId)
        {
            var result = await _carImageRepository.CountAsync(x => x.CarId == carId);
            if (result >= 5)
            {
                return new ErrorResult("Bir arabanın en fazla 5 resmi olabilir.");
            }
            return new SuccessResult();
        }
    }
}
