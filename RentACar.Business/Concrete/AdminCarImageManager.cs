using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Constants;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Helpers.FileHelper;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarImageDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminCarImageManager : IAdminCarImageService
    {
        private readonly ICarImageRepository _carImageRepository;
        private readonly IFileHelper _fileHelper;
        private readonly IMapper _mapper;
        private readonly CarImageBusinessRules _carImageBusinessRules;

        public AdminCarImageManager(ICarImageRepository carImageRepository, IFileHelper fileHelper, IMapper mapper, CarImageBusinessRules carImageBusinessRules)
        {
            _carImageRepository = carImageRepository;
            _fileHelper = fileHelper;
            _mapper = mapper;
            _carImageBusinessRules = carImageBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(CarImageCreateDto carImageAddDto)
        {
            IResult? result = BusinessRules.Run(
                await _carImageBusinessRules.CheckIfCarExistsAsync(carImageAddDto.CarId),
                await _carImageBusinessRules.CheckIfCarImageLimitExceededAsync(carImageAddDto.CarId)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // FİZİKSEL YÜKLEME: Resmi sunucunun (wwwroot) klasörüne yükle.
            string? imagePath = _fileHelper.Upload(carImageAddDto.ImageFile, PathConstants.ImagesPath);
            if (imagePath == null)
            {
                throw new BusinessException("Resim yüklenirken bir hata oluştu veya dosya boş.");
            }

            // VERİTABANI KAYDI
            CarImage carImage = new CarImage
            {
                CarId = carImageAddDto.CarId,
                ImagePath = imagePath
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

            await _carImageRepository.DeleteAsync(result);
            return new SuccessResult("Resim başarıyla silindi.");
        }

        public async Task<IResult> RestoreAsync(int id)
        {
            var deletedImage = await _carImageRepository.GetAsync(x => x.Id == id && x.IsDeleted, ignoreQueryFilters: true);
            if (deletedImage == null)
            {
                throw new BusinessException("Silinmiş resim bulunamadı!");
            }
            deletedImage.IsDeleted = false;
            deletedImage.DeletedDate = null;
            await _carImageRepository.UpdateAsync(deletedImage);
            return new SuccessResult("Resim başarıyla geri yüklendi.");
        }

        public async Task<IDataResult<List<CarImageDeletedDto>>> GetDeletedImagesByCarIdAsync(int carId)
        {
            var deletedImages = await _carImageRepository.GetAllAsync(x => x.CarId == carId && x.IsDeleted, ignoreQueryFilters: true);
            // EF Core liste dönerken asla null dönmez, boş liste döner. Bu yüzden null yerine Any() ile kutunun içini kontrol ediyoruz.
            if (!deletedImages.Any())
            {
                return new SuccessDataResult<List<CarImageDeletedDto>>(new List<CarImageDeletedDto>(), "Çöp kutusu boş.");
            }

            var sortedCarImages = deletedImages.OrderByDescending(x => x.DeletedDate).ToList();
            var dtoList = _mapper.Map<List<CarImageDeletedDto>>(sortedCarImages);
            return new SuccessDataResult<List<CarImageDeletedDto>>(dtoList, "Silinmiş resimler başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto)
        {
            var existingCarImage = await _carImageRepository.GetAsync(x => x.Id == carImageUpdateDto.Id);
            if (existingCarImage == null)
            {
                throw new BusinessException("Resim bulunamadı!");
            }

            string? newImagePath = _fileHelper.Update(carImageUpdateDto.ImageFile, existingCarImage.ImagePath, PathConstants.ImagesPath);
            if (newImagePath == null)
            {
                throw new BusinessException("Resim güncellenirken bir hata oluştu veya dosya boş.");
            }

            existingCarImage.ImagePath = newImagePath;

            await _carImageRepository.UpdateAsync(existingCarImage);
            return new SuccessResult("Resim başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteOldImagesAsync()
        {
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
                await _carImageRepository.HardDeleteByIdAsync(image.Id);
            }
            return new SuccessResult();
        }
    }
}
