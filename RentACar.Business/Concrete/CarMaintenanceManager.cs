using AutoMapper;
using RentACar.Business.Abstract;
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

        public async Task<IResult> AddAsync(CarMaintenanceAddDto carMaintenanceAddDto)
        {
            // Veeritabanından tarih karşılaştırıyoruz o yüzden buraya taşıdık
            carMaintenanceAddDto.CheckInTime = DateTime.SpecifyKind(carMaintenanceAddDto.CheckInTime, DateTimeKind.Utc);
            if (carMaintenanceAddDto.CheckOutTime.HasValue)
            {
                carMaintenanceAddDto.CheckOutTime = DateTime.SpecifyKind(carMaintenanceAddDto.CheckOutTime.Value, DateTimeKind.Utc);
            }

            IResult? result = BusinessRules.Run(
            await CheckIfCarExists(carMaintenanceAddDto.CarId),
            await _carStatusService.CheckIfCarIsRentedAsync(carMaintenanceAddDto.CarId, carMaintenanceAddDto.CheckInTime, carMaintenanceAddDto.CheckOutTime),
            await CheckIfCarAvailableForMaintenance(carMaintenanceAddDto.CarId, carMaintenanceAddDto.CheckInTime, carMaintenanceAddDto.CheckOutTime)
            );
            if (result != null)
            {
                return result;
            }

            var maintenance = _mapper.Map<CarMaintenance>(carMaintenanceAddDto);
            await _carMaintenanceRepository.AddAsync(maintenance);
            return new SuccessResult("Aracın tamir tarihleri başarıyla sisteme kaydedildi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == id);
            if (existingMaintenance == null)
            {
                return new ErrorResult("Silinecek tamir kaydı bulunamadı!");
            }

            existingMaintenance.IsDeleted = true;
            existingMaintenance.DeletedDate = DateTime.UtcNow;
            await _carMaintenanceRepository.UpdateAsync(existingMaintenance);
            return new SuccessResult("Tamir kaydı başaryla silindi.");
        }

        public async Task<IDataResult<List<CarMaintenanceListDto>>> GetAllAsync()
        {
            var maintenances = await _carMaintenanceRepository.GetCarMaintenanceWithDetailsAsync();
            var maintenanceDtos = _mapper.Map<List<CarMaintenanceListDto>>(maintenances);
            return new SuccessDataResult<List<CarMaintenanceListDto>>(maintenanceDtos, "Tamir kayıtları başarıyla listelendi.");
        }

        public async Task<IDataResult<CarMaintenanceListDto>> GetByIdAsync(int id)
        {
            var maintenance = await _carMaintenanceRepository.GetCarMaintenanceByIdWithDetailsAsync(id);
            if (maintenance == null)
            {
                return new ErrorDataResult<CarMaintenanceListDto>("Aranan tamir kaydı bulunamadı!");
            }

            var maintenanceDto = _mapper.Map<CarMaintenanceListDto>(maintenance);
            return new SuccessDataResult<CarMaintenanceListDto>(maintenanceDto, "Tamir kaydı başarıyla geitirildi.");
        }

        public async Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            carMaintenanceUpdateDto.CheckInTime = DateTime.SpecifyKind(carMaintenanceUpdateDto.CheckInTime, DateTimeKind.Utc);
            if (carMaintenanceUpdateDto.CheckOutTime.HasValue)
            {
                carMaintenanceUpdateDto.CheckOutTime = DateTime.SpecifyKind(carMaintenanceUpdateDto.CheckOutTime.Value, DateTimeKind.Utc);
            }

            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == carMaintenanceUpdateDto.Id);
            if (existingMaintenance == null)
            {
                return new ErrorResult("Güncellenecek tamir kaydı bulunamadı!");
            }

            IResult? result = BusinessRules.Run(
                await _carStatusService.CheckIfCarIsRentedAsync(existingMaintenance.CarId, carMaintenanceUpdateDto.CheckInTime, carMaintenanceUpdateDto.CheckOutTime),
                await CheckIfCarAvailableForMaintenanceForUpdate(carMaintenanceUpdateDto.Id, existingMaintenance.CarId, carMaintenanceUpdateDto.CheckInTime, carMaintenanceUpdateDto.CheckOutTime)
            );
            if (result != null)
            {
                return result;
            }

            _mapper.Map(carMaintenanceUpdateDto, existingMaintenance);
            await _carMaintenanceRepository.UpdateAsync(existingMaintenance);
            return new SuccessResult("Tamir kaydı başarıyla güncellendi.");
        }

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
