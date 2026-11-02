using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class OfficeBusinessRules
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IRentalService _rentalService;

        public OfficeBusinessRules(IOfficeRepository officeRepository, IRentalService rentalService)
        {
            _officeRepository = officeRepository;
            _rentalService = rentalService;
        }

        public async Task<IResult> CheckIfOfficeExistsAsync(string officeName)
        {
            bool existingOffice = await _officeRepository.AnyAsync(x => x.Name.ToLower() == officeName.ToLower());
            if (existingOffice)
            {
                return new ErrorResult("Bu ofis sistemde kayıtlı! Lütfen başka ofis girmeyi deneyin.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfOfficeExistsForUpdateAsync(string officeName, int officeId)
        {
            bool isExist = await _officeRepository.AnyAsync(x => x.Name.ToLower() == officeName.ToLower() && x.Id != officeId);
            if (isExist)
            {
                return new ErrorResult("Bu ofis sistemde kayıtlı! Lütfen başka ofis girmeyi deneyin.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfOfficeHasRentalsAsync(int officeId)
        {
            return await _rentalService.CheckIfOfficeHasRentalsAsync(officeId);
        }
    }
}
