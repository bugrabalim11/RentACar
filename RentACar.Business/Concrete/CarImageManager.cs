using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
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

        public async Task<IDataResult<int>> AddAsync(CarImageCreateDto carImageAddDto)
        {
            var car = await _carService.GetByIdAsync(carImageAddDto.CarId);

            // 1. ASİSTAN KONTROLÜ: Kuralları (Limiti) asistana ver.
            IResult? result = BusinessRules.Run(await CheckIfCarImageLimitExceededAsync(carImageAddDto.CarId));
            // 2. KIRMIZI ALARM: Hata varsa asistanın mesajıyla Middleware'i tetikle!
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // 3. FİZİKSEL YÜKLEME: Resmi sunucunun (wwwroot) klasörüne yükle.
            // TODO Refactroing yap
            string? imagePath = _fileHelper.Upload(carImageAddDto.ImageFile, "wwwroot\\Images");
            if (imagePath == null)
            {
                throw new BusinessException("Resim yüklenirken bir hata oluştu veya dosya boş.");
            }

            // 4. VERİTABANI KAYDI
            CarImage carImage = new CarImage
            {
                CarId = carImageAddDto.CarId,
                ImagePath = imagePath,
                UploadDate = DateTime.UtcNow
            };

            await _carImageRepository.AddAsync(carImage);
            return new SuccessDataResult<int>(carImage.Id, "Resim başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var result = await _carImageRepository.GetAsync(x => x.Id == id);
            if (result == null)
            {
                throw new BusinessException("Resim bulunamadı!");
            }

            // SOFT DELETE: Resmi gerçekten silmiyoruz, sadece çöp kutusuna (IsDeleted) atıyoruz.
            // Fiziksel temizliği arka plan servisi (DeleteOldImagesAsync) yapacak.
            result.IsDeleted = true;
            result.DeletedDate = DateTime.UtcNow;
            await _carImageRepository.UpdateAsync(result);
            return new SuccessResult("Resim başarıyla silindi.");
        }

        public async Task<IDataResult<List<CarImageDetailDto>>> GetImagesByCarIdAsync(int carId)
        {
            var car = await _carService.GetByIdAsync(carId);
            var carImages = await _carImageRepository.GetImagesWithCarDetailsAsync(carId);

            // Dolapta hiç resim YOK MU? (Eğer liste boş dönerse)
            if (carImages == null || !carImages.Any())
            {
                // Müşteriye sunulacak "Varsayılan (Default) Resim" tepsisini hazırlıyoruz.
                var defaultDtoList = new List<CarImageDetailDto>
                {
                    new CarImageDetailDto
                    {
                        CarId = carId,
                        // TODO Refactroing yap
                        ImagePath = "wwwroot\\Images\\default.jpg",
                        UploadDate = DateTime.UtcNow,
                        CarName = $"{car.Data?.BrandName} {car.Data?.ModelName}"
                    }
                };

                // Erken Çıkış (Early Return): Sahte listeyi kuryeye verip metodu burada bitiriyoruz.
                return new SuccessDataResult<List<CarImageDetailDto>>(defaultDtoList, "Bu araca ait resim bulunamadı, varsayılan resim getirildi.");
            }

            // Robot, çiğ etleri (carImages) alıp, Profile dosyasındaki tarifine göre pişirip DTO tepsisine diziyor.
            var dtoList = _mapper.Map<List<CarImageDetailDto>>(carImages);
            return new SuccessDataResult<List<CarImageDetailDto>>(dtoList, "Bu araca ait resimler başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto)
        {
            var car = await _carService.GetByIdAsync(carImageUpdateDto.CarId);
            var existingCarImage = await _carImageRepository.GetAsync(x => x.Id == carImageUpdateDto.Id);
            if (existingCarImage == null)
            {
                throw new BusinessException("Resim bulunamadı!");
            }

            string? newImagePath = _fileHelper.Update(carImageUpdateDto.ImageFile, existingCarImage.ImagePath, "wwwroot\\Images");
            if (newImagePath == null)
            {
                throw new BusinessException("Resim güncellenirken bir hata oluştu veya dosya boş.");
            }

            existingCarImage.ImagePath = newImagePath;
            existingCarImage.UploadDate = DateTime.UtcNow;

            await _carImageRepository.UpdateAsync(existingCarImage);
            return new SuccessResult("Resim başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteOldImagesAsync()
        {
            // KARANTİNA KURALI: Sadece silinmiş (IsDeleted) olanları VE 
            // çöp kutusunda 30 günden fazla beklemiş olanları (karantina süresi dolanları) getir.
            var oldImages = await _carImageRepository.GetAllAsync(x => x.IsDeleted && x.DeletedDate < DateTime.UtcNow.AddDays(-30), ignoreQueryFilters: true);

            // KORUMA KALKANI (Guard Clause / Early Return): 
            // Eğer silinecek resim yoksa gereksiz yere foreach döngüsüne girmemek için kapıdan dön.
            if (oldImages == null || !oldImages.Any())
            {
                return new SuccessResult("Silinecek eski resim bulunamadı.");
            }

            foreach (var image in oldImages)
            {
                // ÖNCE FİZİKSEL TEMİZLİK: Sunucunun klasöründeki asıl JPG/PNG dosyasını uçuruyoruz.
                _fileHelper.Delete(image.ImagePath);

                // SONRA VERİTABANI TEMİZLİĞİ: SQL'den o kaydı kalıcı olarak (Hard Delete) siliyoruz.
                await _carImageRepository.DeleteAsync(image);
            }
            return new SuccessResult();
        }

        // --- İÇ RAPORLAMA MERKEZİ (KURAL USTALARI) ---
        // Sadece Manager'ın okuması için rapor (ErrorResult) dönerler. Middleware'i tetiklemezler.

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