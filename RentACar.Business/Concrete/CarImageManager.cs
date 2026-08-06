using AutoMapper;
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
        private readonly IMapper _mapper;

        public CarImageManager(ICarImageRepository carImageRepository, IFileHelper fileHelper, ICarService carService, IMapper mapper)
        {
            _carImageRepository = carImageRepository;
            _fileHelper = fileHelper;
            _carService = carService;
            _mapper = mapper;
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

            result.IsDeleted = true;
            result.DeletedDate = DateTime.UtcNow;
            await _carImageRepository.UpdateAsync(result);
            return new SuccessResult("Resim başarıyla silindi.");
        }

        public async Task<IDataResult<List<CarImageDetailDto>>> GetImagesByCarIdAsync(int carId)
        {
            IResult? result = BusinessRules.Run(await CheckIfCarExists(carId));
            if (result != null)
            {
                return new ErrorDataResult<List<CarImageDetailDto>>(result.Message ?? "Araç resimlerini getirilirken hata oluştu!");
            }

            var carImages = await _carImageRepository.GetImagesWithCarDetailsAsync(carId);
            if (carImages == null)
            {
                return new ErrorDataResult<List<CarImageDetailDto>>("Bu araca ait resimler bulunamadı!");
            }

            // Dolapta hiç resim YOK MU?
            if (!carImages.Any())
            {
                var carResult = await _carService.GetByIdAsync(carId);
                // Müşteriye sunulacak porselen tabak (DTO Listesi) hazırlıyoruz
                var defaultDtoList = new List<CarImageDetailDto>
                {
                    new CarImageDetailDto
                    {
                        CarId = carId,
                        ImagePath = "wwwroot\\Images\\default.jpg",
                        UploadDate = DateTime.UtcNow,
                        CarName = $"{carResult.Data?.BrandName} {carResult.Data?.ModelName}"
                    }
                };

                // Sahte listeyi kuryeye verip metodu DİREKT burada bitiriyoruz. (Aşağıya inmez)
                return new SuccessDataResult<List<CarImageDetailDto>>(defaultDtoList, "Bu araca ait resim bulunamadı, varsayılan resim getirildi.");
            }

            // Robot, çiğ etleri (carImages) alıp, Profile dosyasındaki tarifine göre pişirip DTO tepsisine diziyor.
            var dtoList = _mapper.Map<List<CarImageDetailDto>>(carImages);

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

        public async Task<IResult> DeleteOldImagesAsync()
        {
            // KURAL: Sadece silinmiş (IsDeleted) olanları VE 
            // çöp kutusunda 30 günden fazla beklemiş olanları (karantina süresi dolanları) getir.
            var oldImages = await _carImageRepository.GetAllAsync(x => x.IsDeleted && x.DeletedDate < DateTime.UtcNow.AddMinutes(-1));

            // GÜVENLİK KAPISI (Bekçiyi boş yere yormamak için)
            // Eğer liste hiç oluşmadıysa (null) VEYA listenin içinde hiç eleman YOKSA (!Any)
            if (oldImages == null || !oldImages.Any())
            {
                return new SuccessResult("Silinecek eski resim bulunamadı.");
            }

            // ADIM: Çöp torbalarını tek tek açıyoruz.
            foreach (var image in oldImages)
            {
                // ÖNCE FİZİKSEL TEMİZLİK: Sunucunun (wwwroot/images) klasöründeki asıl JPG/PNG dosyasını uçuruyoruz.
                _fileHelper.Delete(image.ImagePath);

                // SONRA VERİTABANI TEMİZLİĞİ: SQL'den o kaydı kalıcı olarak (Hard Delete) siliyoruz.
                await _carImageRepository.DeleteAsync(image);
            }
            return new SuccessResult();
        }
    }
}
