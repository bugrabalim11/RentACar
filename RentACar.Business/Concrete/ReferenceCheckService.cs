using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class ReferenceCheckService : IReferenceCheckService
    {
        private readonly IRentalRepository _rentalRepository;

        public ReferenceCheckService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<IResult> CheckIfOfficeHasRentalsAsync(int officeId)
        {
            var checkRental = await _rentalRepository.AnyAsync(x => x.PickUpOfficeId == officeId || x.DropOffOfficeId == officeId);
            if (!checkRental)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Ofise ait kiralama işlemleri mevcut, bu yüzden silinemez!");
        }
    }
}
