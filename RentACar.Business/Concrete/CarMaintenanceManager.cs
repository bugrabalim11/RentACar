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

        public Task<IResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<CarMaintenanceListDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
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
