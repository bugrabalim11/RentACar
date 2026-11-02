using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.Concrete
{
    public class CarMaintenanceManager : ICarMaintenanceService
    {
        private readonly ICarMaintenanceRepository _carMaintenanceRepository;
        private readonly IMapper _mapper;

        public CarMaintenanceManager(ICarMaintenanceRepository carMaintenanceRepository, IMapper mapper)
        {
            _carMaintenanceRepository = carMaintenanceRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<CarMaintenanceResultDto>>> GetAllAsync()
        {
            var maintenances = await _carMaintenanceRepository.GetCarMaintenanceWithDetailsAsync();
            var sortedCarMaintenances = maintenances.OrderByDescending(x => x.CreatedDate).ToList();
            var maintenanceDtos = _mapper.Map<List<CarMaintenanceResultDto>>(sortedCarMaintenances);
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
    }
}