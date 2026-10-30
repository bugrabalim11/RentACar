using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;

namespace RentACar.Business.Concrete
{
    public class CarManager : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly CarBusinessRules _carBusinessRules;

        public CarManager(ICarRepository carRepository, IMapper mapper, CarBusinessRules carBusinessRules)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _carBusinessRules = carBusinessRules;
        }

        public async Task<IDataResult<List<CarResultDto>>> GetAllAsync()
        {
            var cars = await _carRepository.GetCarsWithDetailsAsync();
            var sortedCars = cars.OrderByDescending(x => x.CreatedDate).ToList();
            var carListDtos = _mapper.Map<List<CarResultDto>>(sortedCars);
            return new SuccessDataResult<List<CarResultDto>>(carListDtos, "Arabalar başarıyla listelendi.");
        }

        public async Task<IDataResult<CarDetailDto>> GetByIdAsync(int id)
        {
            var car = await _carRepository.GetCarWithDetailsAsync(id);
            if (car == null)
            {
                throw new BusinessException("Aranan araç detayı bulunamadı.");
            }

            var carDetailDto = _mapper.Map<CarDetailDto>(car);
            return new SuccessDataResult<CarDetailDto>(carDetailDto, "Araba detayı getirildi.");
        }

        public async Task<IDataResult<List<CarResultDto>>> GetAllByBrandIdAsync(int brandId)
        {
            var existingCars = await _carRepository.GetAllAsync(x => x.BrandId == brandId);
            var mappedCars = _mapper.Map<List<CarResultDto>>(existingCars);
            return new SuccessDataResult<List<CarResultDto>>(mappedCars);
        }

        public async Task<IDataResult<List<CarResultDto>>> GetCarsByColorIdAsync(int colorId)
        {
            var existingCars = await _carRepository.GetAllAsync(x => x.ColorId == colorId);
            var mappedCars = _mapper.Map<List<CarResultDto>>(existingCars);
            return new SuccessDataResult<List<CarResultDto>>(mappedCars);
        }

        public async Task<IResult> CheckIfCarExistsAsync(int carId)
        {
            bool isExist = await _carRepository.AnyAsync(x => x.Id == carId);
            if (!isExist)
            {
                return new ErrorResult("Böyle bir araç sistemde bulunamadı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId)
        {
            return await _carBusinessRules.CheckIfBrandIsUsedByAnyCarAsync(brandId);
        }
    }
}
