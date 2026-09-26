using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarMaintenanceDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class CarMaintenanceManager : ICarMaintenanceService
    {
        private readonly ICarMaintenanceRepository _carMaintenanceRepository;
        private readonly IMapper _mapper;
        private readonly ICarService _carService;
        private readonly ICarStatusService _carStatusService;

        public CarMaintenanceManager(ICarMaintenanceRepository carMaintenanceRepository, IMapper mapper, ICarService carService, ICarStatusService carStatusService)
        {
            _carMaintenanceRepository = carMaintenanceRepository;
            _mapper = mapper;
            _carService = carService;
            _carStatusService = carStatusService;
        }

        public async Task<IDataResult<int>> AddAsync(CarMaintenanceCreateDto carMaintenanceAddDto)
        {
            // 1. ZAMAN YOLCUSU AYARI (Timezone): PostgreSQL UTC saat formatı ister. 
            // Garsonun getirdiği bu tarihe "Bu Evrensel (UTC) bir tarihtir" etiketini (mührünü) basıyoruz.
            carMaintenanceAddDto.CheckInTime = DateTime.SpecifyKind(carMaintenanceAddDto.CheckInTime, DateTimeKind.Utc);
            if (carMaintenanceAddDto.CheckOutTime.HasValue)
            {
                carMaintenanceAddDto.CheckOutTime = DateTime.SpecifyKind(carMaintenanceAddDto.CheckOutTime.Value, DateTimeKind.Utc);
            }

            // 2. KONTROL ASİSTANI (BusinessRules): Kuralları asistana ver.
            IResult? result = BusinessRules.Run(
                await CheckIfCarExists(carMaintenanceAddDto.CarId),
                await _carStatusService.CheckIfCarIsRentedAsync(carMaintenanceAddDto.CarId, carMaintenanceAddDto.CheckInTime, carMaintenanceAddDto.CheckOutTime),
                await CheckIfCarAvailableForMaintenance(carMaintenanceAddDto.CarId, carMaintenanceAddDto.CheckInTime, carMaintenanceAddDto.CheckOutTime)
            );

            // 3. KIRMIZI ALARM: Asistan hata raporuyla dönerse sistemi durdur ve şefi (Middleware) çağır!
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // 4. İŞLEM: DTO'yu Entity'e çevir ve veritabanına kaydet.
            var maintenance = _mapper.Map<CarMaintenance>(carMaintenanceAddDto);
            await _carMaintenanceRepository.AddAsync(maintenance);
            return new SuccessDataResult<int>(maintenance.Id, "Aracın tamir tarihleri başarıyla sisteme kaydedildi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == id);
            if (existingMaintenance == null)
            {
                throw new BusinessException("Silinecek tamir kaydı bulunamadı!");
            }

            // SOFT DELETE (Yumuşak Silme): Gerçekten silmiyoruz, üzerini çizip çöp kutusuna atıyoruz.
            existingMaintenance.IsDeleted = true;
            existingMaintenance.DeletedDate = DateTime.UtcNow;

            await _carMaintenanceRepository.UpdateAsync(existingMaintenance);
            return new SuccessResult("Tamir kaydı başaryla silindi.");
        }

        public async Task<IDataResult<List<CarMaintenanceResultDto>>> GetAllAsync()
        {
            var maintenances = await _carMaintenanceRepository.GetCarMaintenanceWithDetailsAsync();
            var maintenanceDtos = _mapper.Map<List<CarMaintenanceResultDto>>(maintenances);
            return new SuccessDataResult<List<CarMaintenanceResultDto>>(maintenanceDtos, "Tamir kayıtları başarıyla listelendi.");
        }

        public async Task<IDataResult<CarMaintenanceResultDto>> GetByIdAsync(int id)
        {
            var maintenance = await _carMaintenanceRepository.GetCarMaintenanceByIdWithDetailsAsync(id);
            if (maintenance == null)
            {
                throw new BusinessException("Aranan tamir kaydı bulunamadı!");
            }

            var maintenanceDto = _mapper.Map<CarMaintenanceResultDto>(maintenance);
            return new SuccessDataResult<CarMaintenanceResultDto>(maintenanceDto, "Tamir kaydı başarıyla geitirildi.");
        }

        public async Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            // 1. ZAMAN YOLCUSU AYARI (Timezone): Tarihlere UTC mührünü basıyoruz.
            carMaintenanceUpdateDto.CheckInTime = DateTime.SpecifyKind(carMaintenanceUpdateDto.CheckInTime, DateTimeKind.Utc);
            if (carMaintenanceUpdateDto.CheckOutTime.HasValue)
            {
                carMaintenanceUpdateDto.CheckOutTime = DateTime.SpecifyKind(carMaintenanceUpdateDto.CheckOutTime.Value, DateTimeKind.Utc);
            }

            // 2. KİMLİK KONTROLÜ: Güncellenecek kayıt var mı?
            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == carMaintenanceUpdateDto.Id);
            if (existingMaintenance == null)
            {
                throw new BusinessException("Güncellenecek tamir kaydı bulunamadı!");
            }

            // 3. KONTROL ASİSTANI (BusinessRules)
            IResult? result = BusinessRules.Run(
                await _carStatusService.CheckIfCarIsRentedAsync(existingMaintenance.CarId, carMaintenanceUpdateDto.CheckInTime, carMaintenanceUpdateDto.CheckOutTime),
                await CheckIfCarAvailableForMaintenanceForUpdate(carMaintenanceUpdateDto.Id, existingMaintenance.CarId, carMaintenanceUpdateDto.CheckInTime, carMaintenanceUpdateDto.CheckOutTime)
            );

            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // 4. İŞLEM: DTO'dan gelen yeni verileri, mevcut veritabanı nesnesinin üzerine kopyala.
            _mapper.Map(carMaintenanceUpdateDto, existingMaintenance);
            await _carMaintenanceRepository.UpdateAsync(existingMaintenance);
            return new SuccessResult("Tamir kaydı başarıyla güncellendi.");
        }

        // --- İÇ RAPORLAMA MERKEZİ (KURAL USTALARI) ---
        // Sadece Manager'ın okuması için rapor (ErrorResult) dönerler. Middleware'i tetiklemezler.

        public async Task<IResult> CheckIfCarAvailableForMaintenance(int carId, DateTime checkInTime, DateTime? checkOutTime)
        {
            bool isExist = await _carMaintenanceRepository
                .AnyAsync(x => x.CarId == carId && (x.CheckOutTime == null || checkInTime <= x.CheckOutTime) && (checkOutTime == null || checkOutTime >= x.CheckInTime));
            if (isExist)
            {
                return new ErrorResult("Bu araç, seçilen tarihler arasında zaten sanayidedir!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfCarAvailableForMaintenanceForUpdate(int maintenanceId, int carId, DateTime checkInTime, DateTime? checkOutTime)
        {
            // Güncelleme sırasında kendi ID'sini (x.Id != maintenanceId) kontrolden hariç tutar.
            bool isExist = await _carMaintenanceRepository.AnyAsync(x => x.CarId == carId && (x.Id != maintenanceId) && (x.CheckOutTime == null || checkInTime <= x.CheckOutTime) && (checkOutTime == null || checkOutTime >= x.CheckInTime));
            if (isExist)
            {
                return new ErrorResult("Bu araç, güncellenmek istenen tarihler arasında zaten sanayidedir!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfCarExists(int carId)
        {
            var result = await _carService.CheckIfCarExistsAsync(carId);
            if (!result.Success)
            {
                return new ErrorResult("Böyle bir araç sistemde bulunamadı!");
            }
            return new SuccessResult();
        }
    }
}