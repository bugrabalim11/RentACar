using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Aspects.Autofac.Transaction;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarMaintenanceDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminCarMaintenanceManager : IAdminCarMaintenanceService
    {
        private readonly ICarMaintenanceRepository _carMaintenanceRepository;
        private readonly IMapper _mapper;
        private readonly CarMaintenanceBusinessRules _carMaintenanceBusinessRules;

        public AdminCarMaintenanceManager(ICarMaintenanceRepository carMaintenanceRepository, IMapper mapper, CarMaintenanceBusinessRules carMaintenanceBusinessRules)
        {
            _carMaintenanceRepository = carMaintenanceRepository;
            _mapper = mapper;
            _carMaintenanceBusinessRules = carMaintenanceBusinessRules;
        }

        [TransactionScopeAspect]
        public async Task<IDataResult<int>> AddAsync(CarMaintenanceCreateDto carMaintenanceAddDto)
        {
            carMaintenanceAddDto.CheckInTime = carMaintenanceAddDto.CheckInTime.ToUniversalTime();
            if (carMaintenanceAddDto.CheckOutTime.HasValue)
            {
                carMaintenanceAddDto.CheckOutTime = carMaintenanceAddDto.CheckOutTime.Value.ToUniversalTime();
            }

            IResult? result = BusinessRules.Run(
                await _carMaintenanceBusinessRules.CheckIfCarExists(carMaintenanceAddDto.CarId),
                await _carMaintenanceBusinessRules.CheckIfCarIsRentedAsync(carMaintenanceAddDto.CarId, carMaintenanceAddDto.CheckInTime, carMaintenanceAddDto.CheckOutTime),
                await _carMaintenanceBusinessRules.CheckIfCarAvailableForMaintenance(carMaintenanceAddDto.CarId, carMaintenanceAddDto.CheckInTime, carMaintenanceAddDto.CheckOutTime)
            );

            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var maintenance = _mapper.Map<CarMaintenance>(carMaintenanceAddDto);
            await _carMaintenanceRepository.AddAsync(maintenance);
            return new SuccessDataResult<int>(maintenance.Id, "Aracın tamir tarihleri başarıyla sisteme kaydedildi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> DeleteAsync(int id)
        {
            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == id);
            if (existingMaintenance == null)
            {
                throw new BusinessException("Silinecek tamir kaydı bulunamadı!");
            }

            await _carMaintenanceRepository.DeleteAsync(existingMaintenance);
            return new SuccessResult("Tamir kaydı başaryla silindi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            carMaintenanceUpdateDto.CheckInTime = carMaintenanceUpdateDto.CheckInTime.ToUniversalTime();
            if (carMaintenanceUpdateDto.CheckOutTime.HasValue)
            {
                carMaintenanceUpdateDto.CheckOutTime = carMaintenanceUpdateDto.CheckOutTime.Value.ToUniversalTime();
            }

            var existingMaintenance = await _carMaintenanceRepository.GetAsync(x => x.Id == carMaintenanceUpdateDto.Id);
            if (existingMaintenance == null)
            {
                throw new BusinessException("Güncellenecek tamir kaydı bulunamadı!");
            }

            IResult? result = BusinessRules.Run(
                await _carMaintenanceBusinessRules.CheckIfCarIsRentedAsync(existingMaintenance.CarId, carMaintenanceUpdateDto.CheckInTime, carMaintenanceUpdateDto.CheckOutTime),
                await _carMaintenanceBusinessRules.CheckIfCarAvailableForMaintenanceForUpdate(carMaintenanceUpdateDto.Id, existingMaintenance.CarId, carMaintenanceUpdateDto.CheckInTime, carMaintenanceUpdateDto.CheckOutTime)
            );

            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(carMaintenanceUpdateDto, existingMaintenance);
            await _carMaintenanceRepository.UpdateAsync(existingMaintenance);
            return new SuccessResult("Tamir kaydı başarıyla güncellendi.");
        }
    }
}
