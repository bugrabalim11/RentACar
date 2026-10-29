using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminCarManager : IAdminCarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly CarBusinessRules _carBusinessRules;

        public AdminCarManager(ICarRepository carRepository, IMapper mapper, CarBusinessRules carBusinessRules)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _carBusinessRules = carBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(CarCreateDto carAddDto)
        {
            carAddDto.Plate = carAddDto.Plate.Replace(" ", "").ToUpper();

            IResult? result = BusinessRules.Run(
                await _carBusinessRules.CheckIfCarPlateExistsAsync(carAddDto.Plate),
                await _carBusinessRules.CheckIfBrandExistsAsync(carAddDto.BrandId),
                await _carBusinessRules.CheckIfColorExistsAsync(carAddDto.ColorId)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var car = _mapper.Map<Car>(carAddDto);
            await _carRepository.AddAsync(car);
            return new SuccessDataResult<int>(car.Id, "Araç başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingCar = await _carRepository.GetAsync(x => x.Id == id);
            if (existingCar == null)
            {
                throw new BusinessException("Silincek araç bulunamadı.");
            }

            // SENİOR NOTU: Biz sadece silme emrini (DeleteAsync) veriyoruz. 
            // Fiziksel silme (Hard Delete) mi yoksa çöpe atma (Soft Delete) mı yapılacağına 
            // DataAccess katmanındaki DbContext (Gümrük Memuru) otonom olarak karar verir!
            await _carRepository.DeleteAsync(existingCar);
            return new SuccessResult("Araç başarıyla silindi.");
        }

        public async Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto)
        {
            var existingCar = await _carRepository.GetAsync(x => x.Id == carUpdateDto.Id);
            if (existingCar == null)
            {
                throw new BusinessException("Güncellencek araç bulunamadı.");
            }

            carUpdateDto.Plate = carUpdateDto.Plate.Replace(" ", "").ToUpper();

            IResult? result = BusinessRules.Run(
                await _carBusinessRules.CheckIfCarPlateExitsForUpdateAsync(carUpdateDto.Plate, carUpdateDto.Id),
                await _carBusinessRules.CheckIfBrandExistsAsync(carUpdateDto.BrandId),
                await _carBusinessRules.CheckIfColorExistsAsync(carUpdateDto.ColorId)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(carUpdateDto, existingCar);
            await _carRepository.UpdateAsync(existingCar);
            return new SuccessResult("Araç başarıyla güncellendi.");
        }
    }
}
