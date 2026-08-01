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
        private readonly ICarService _carService;

        public CarImageManager(ICarImageRepository carImageRepository, IFileHelper fileHelper, ICarService carService)
        {
            _carImageRepository = carImageRepository;
            _fileHelper = fileHelper;
            _carService = carService;
        }

        public async Task<IResult> AddAsync(CarImageAddDto carImageAddDto)
        {
            IResult? result = BusinessRules.Run(
            await CheckIfCarImageLimitExceededAsync(carImageAddDto.CarId),
            await CheckIfCarExists(carImageAddDto.CarId)
            );
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

        public async Task<IDataResult<List<CarImageDetailDto>>> GetImagesByCarIdAsync(int carId)
        {
            var carImages = await _carImageRepository.GetAllAsync(x => x.CarId == carId);
            if (carImages == null)
            {
                return new ErrorDataResult<List<CarImageDetailDto>>("Bu araca ait resimler bulunamadı!");
            }

            // Dolapta hiç resim YOK MU?
            if (!carImages.Any())
            {
                // Müşteriye sunulacak porselen tabak (DTO Listesi) hazırlıyoruz
                var defaultDtoList = new List<CarImageDetailDto>
                {
                    new CarImageDetailDto
                    {
                        CarId = carId,
                        ImagePath = "wwwroot\\Images\\default.jpg",
                        UploadDate = DateTime.UtcNow
                    }
                };

                // Sahte listeyi kuryeye verip metodu DİREKT burada bitiriyoruz. (Aşağıya inmez)
                return new SuccessDataResult<List<CarImageDetailDto>>(defaultDtoList, "Bu araca ait resim bulunamadı, varsayılan resim getirildi.");
            }

            // Kod buraya indiyse dolap doludur! Çiğ etleri DTO'ya çeviriyoruz (Mapping)

            // Boş bir porselen tabak listesi hazırladık
            var dtoList = new List<CarImageDetailDto>();

            // Dolaptaki her bir çiğ eti alıp...
            foreach (var image in carImages)
            {
                // ...pişirip yeni formata (DTO'ya) sokuyoruz
                var mappedDto = new CarImageDetailDto
                {
                    Id = image.Id,
                    CarId = image.CarId,
                    ImagePath = image.ImagePath,
                    UploadDate = image.UploadDate
                };

                // Pişen yemeği porselen tabağa (Listeye) ekliyoruz
                dtoList.Add(mappedDto);
            }

            return new SuccessDataResult<List<CarImageDetailDto>>(dtoList, "Bu araca ait resimler başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto)
        {
            IResult? result = BusinessRules.Run(await CheckIfCarExists(carImageUpdateDto.CarId));
            if (result != null)
            {
                return result;
            }

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

        private async Task<IResult> CheckIfCarExists(int carId)
        {
            var result = await _carService.GetByIdAsync(carId);
            if (!result.Success)
            {
                return new ErrorResult("Araba bulunamadı.");
            }
            return new SuccessResult();
        }
    }
}
