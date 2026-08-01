using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class CarManager : ICarService
    {
        // ESKİSİ: private readonly IRepository<Car> _carRepository;
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;  // İşte bizim yetenekli aşçı yamağımız!

        // Başka bir domain'in (Marka) veritabanına (Repository) doğrudan erişmek N-Tier mimarisine aykırıdır.
        // Bu yüzden o dükkanın yöneticisi (IBrandService) DI üzerinden talep edilerek iletişim köprüsü kurulmuştur.
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;

        // 2. Constructor'a (Yapıcı Metot) ekleyerek sisteme "Bana bu görevliyi getir" diyoruz.
        public CarManager(ICarRepository carRepository, IMapper mapper, IBrandService brandService, IColorService colorService)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _brandService = brandService;
            _colorService = colorService;
        }

        public async Task<IResult> AddAsync(CarAddDto carAddDto)
        {
            // 2. İŞ KURALLARI (Business Rules - Dükkanın mantık kuralları)
            carAddDto.Plate = carAddDto.Plate.Replace(" ", "").ToUpper();

            IResult? result = BusinessRules.Run(
            await CheckIfBrandExistsAsync(carAddDto.BrandId),
            await CheckIfColorExistsAsync(carAddDto.ColorId),
            await CheckIfCarPlateExistsAsync(carAddDto.Plate)
            );

            if (result != null)
            {
                return result;
            }

            // 3. KAYIT (Her şey tamamsa yemeği pişir)
            var car = _mapper.Map<Car>(carAddDto);
            await _carRepository.AddAsync(car);
            return new SuccessResult("Araç başarıyla eklendi.");
        }

        // TODO: İleride Hangfire/Quartz kurularak, silinen arabaların fiziki resimlerini
        // 30 gün sonra temizleyen bir Background Job yazılacak.
        public async Task<IResult> DeleteAsync(int id)
        {
            var existingCar = await _carRepository.GetAsync(x => x.Id == id);
            if (existingCar == null)
            {
                return new ErrorResult("Silincek araç bulunamadı.");
            }

            existingCar.Status = false;
            await _carRepository.UpdateAsync(existingCar);
            return new SuccessResult("Araç başarıyla silindi.");
        }

        public async Task<IDataResult<List<CarListDto>>> GetAllAsync()
        {
            // İşte senin DataAccess'te yazdığın o özel Join'li metodu çağırıyoruz!
            var cars = await _carRepository.GetCarsWithDetailsAsync();

            // Arabalar, markaları ve renkleriyle beraber geldi. Şimdi onları şık tabaklara (DTO) koyalım.
            var carListDtos = _mapper.Map<List<CarListDto>>(cars);

            // Kargo kutusuna koy ve yolla!
            return new SuccessDataResult<List<CarListDto>>(carListDtos, "Arabalar başarıyla listelendi.");
        }


        public async Task<IDataResult<CarDetailDto>> GetByIdAsync(int id)
        {
            // ESKİSİ: var car = await _carRepository.GetAsync(x => x.Id == id);
            // YENİSİ: Artık Join'li veriyi getiren kendi özel metodumuzu kullanıyoruz!
            var car = await _carRepository.GetCarWithDetailsAsync(id);
            if (car == null)
            {
                return new ErrorDataResult<CarDetailDto>("Aranan araç detayı bulunamadı.");
            }

            // Bulduysa CarListDto'ya çevirir, bulamadıysa (null ise) güvenli bir şekilde null döner
            var carDetailDto = _mapper.Map<CarDetailDto>(car);
            return new SuccessDataResult<CarDetailDto>(carDetailDto, "Araba detayı getirildi.");
        }

        public async Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto)
        {
            // Replace -> boşluklaarı kapatır ToUpper-> büyük harf yapar
            carUpdateDto.Plate = carUpdateDto.Plate.Replace(" ", "").ToUpper();

            IResult? result = BusinessRules.Run(
            await CheckIfBrandExistsAsync(carUpdateDto.BrandId),
            await CheckIfColorExistsAsync(carUpdateDto.ColorId),
            await CheckIfCarPlateExitsForUpdateAsync(carUpdateDto.Plate, carUpdateDto.Id)
            );
            if (result != null)
            {
                return result;
            }

            // 3. VERİTABANI KONTROLÜ (Güncellenecek araba gerçekten var mı?)
            var existingCar = await _carRepository.GetAsync(x => x.Id == carUpdateDto.Id);
            if (existingCar == null)
            {
                return new ErrorResult("Güncellencek araç bulunamadı.");
            }

            // 4. EŞLEŞTİRME VE KAYIT
            _mapper.Map(carUpdateDto, existingCar);
            await _carRepository.UpdateAsync(existingCar);
            return new SuccessResult("Araç başarıyla güncellendi.");
        }

        // İş Kuralı: Aynı plaka var mı kontrolü
        private async Task<IResult> CheckIfCarPlateExistsAsync(string plate)
        {

            bool existingPlate = await _carRepository.AnyAsync(x => x.Plate == plate);
            if (existingPlate)
            {
                return new ErrorResult("Bu plaka zaten sistemde kayıtlı!");
            }
            return new SuccessResult();
        }

        // İş Kuralı: Güncelleme yaparken başka bir arabaya ait aynı plaka var mı kontrolü
        private async Task<IResult> CheckIfCarPlateExitsForUpdateAsync(string plate, int currentCarId)
        {

            bool isExist = await _carRepository.AnyAsync(x => x.Plate == plate && x.Id != currentCarId);

            if (isExist)
            {
                return new ErrorResult("Bu plaka zaten sistemde kayıtlı.");
            }
            return new SuccessResult();
        }


        private async Task<IResult> CheckIfBrandExistsAsync(int brandId)
        {
            var existingBrand = await _brandService.CheckIfBrandExistsAsync(brandId);
            if (!existingBrand.Success)
            {
                // İş Kuralı Güvenliği (Defensive Programming):
                // Eğer result.Message sistemden boş (null) gelirse, sistemin NullReferenceException ile çökmesini engellemek için 
                // '??' (Null-Coalescing / Yedek Paraşüt) operatörü kullanılarak varsayılan bir hata mesajı atanmıştır.
                return new ErrorResult(existingBrand.Message ?? "Bilinmeyen bir marka hatası oluştu!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfColorExistsAsync(int colorId)
        {
            var existingColor = await _colorService.CheckIfColorExistsAsync(colorId);
            if (!existingColor.Success)
            {
                return new ErrorResult(existingColor.Message ?? "Bilinmeyen bir renk hatası oluştu!");
            }
            return new SuccessResult();
        }
    }
}
