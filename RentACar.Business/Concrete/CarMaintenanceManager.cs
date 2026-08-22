using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.Concrete
{
    public class CarMaintenanceManager : ICarMaintenanceService
    {
        private readonly ICarMaintenanceRepository _carMaintenanceRepository;

        public CarMaintenanceManager(ICarMaintenanceRepository carMaintenanceRepository)
        {
            _carMaintenanceRepository = carMaintenanceRepository;
        }

        public Task<IResult> AddAsync(CarMaintenanceAddDto carMaintenanceAddDto)
        {
            throw new NotImplementedException();
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
    }
}
