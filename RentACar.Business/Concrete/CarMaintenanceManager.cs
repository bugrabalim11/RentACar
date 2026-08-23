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

        public CarMaintenanceManager(ICarMaintenanceRepository carMaintenanceRepository, IMapper mapper, ICarService carService)
        {
            _carMaintenanceRepository = carMaintenanceRepository;
            _mapper = mapper;
            _carService = carService;
        }

        public async Task<IResult> AddAsync(CarMaintenanceAddDto carMaintenanceAddDto)
        {
            IResult? result = BusinessRules.Run(
            await CheckIfCarExists(carMaintenanceAddDto.CarId),
            await CheckIfCarIsAlreadyInMaintenance(carMaintenanceAddDto.CarId)
            );
            if (result != null)
            {
                return result;
            }

            var maintenance = _mapper.Map<CarMaintenance>(carMaintenanceAddDto);
            // Saatleri ŞİMDİ Entity üzerinde UTC'ye çevir!
            carMaintenanceAddDto.CheckInTime = DateTime.SpecifyKind(carMaintenanceAddDto.CheckInTime, DateTimeKind.Utc);
            if (carMaintenanceAddDto.CheckOutTime.HasValue)
            {
                carMaintenanceAddDto.CheckOutTime = DateTime.SpecifyKind(carMaintenanceAddDto.CheckOutTime.Value, DateTimeKind.Utc);
            }
            await _carMaintenanceRepository.AddAsync(maintenance);
            return new SuccessResult("Aracın tamir tarihleri başarıyla sisteme kaydedildi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == id);
            if (existingMaintenance == null)
            {
                return new ErrorResult("Silinecek tamir bulunamadı!");
            }

            existingMaintenance.IsDeleted = true;
            existingMaintenance.DeletedDate = DateTime.UtcNow;
            await _carMaintenanceRepository.UpdateAsync(existingMaintenance);
            return new SuccessResult("Tamir başaryla silindi.");
        }

        public async Task<IDataResult<List<CarMaintenanceListDto>>> GetAllAsync()
        {
            var maintenances = await _carMaintenanceRepository.GetCarMaintenanceWithDetailsAsync();
            var maintenanceDtos = _mapper.Map<List<CarMaintenanceListDto>>(maintenances);
            return new SuccessDataResult<List<CarMaintenanceListDto>>(maintenanceDtos, "Tamirler başarıyla listelendi.");
        }

        public Task<IDataResult<CarMaintenanceListDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            throw new NotImplementedException();
        }

        private async Task<IResult> CheckIfCarIsAlreadyInMaintenance(int carId)
        {
            bool result = await _carMaintenanceRepository.AnyAsync(x => x.CarId == carId && x.CheckOutTime == null);
            if (result)
            {
                return new ErrorResult("Bu araç şu an zaten bakımda!");
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
