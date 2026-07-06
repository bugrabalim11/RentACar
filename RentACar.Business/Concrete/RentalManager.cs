using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRepository<Rental> _rentalRepository;

        public RentalManager(IRepository<Rental> rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task AddAsync(Rental rental)
        {
            // İŞ KURALI (BUSINESS RULE): Araba şu an müsait mi?
            // Veritabanına gidip soruyoruz: "Bu CarId'ye sahip ve ReturnDate'i (Dönüş Tarihi) NULL olan bir kayıt var mı?"
            var isCarRented = await _rentalRepository.GetAsync(r => r.CarId == rental.CarId && r.ReturnDate == null);

            // Eğer isCarRented değişkeni 'null' DÖNMEDİYSE (yani içerisi doluysa), demek ki araba şu an dışarıda!
            if (isCarRented != null) // != Eşit değilse demek
            {
                throw new Exception("Bu araç şu an başka bir müşteride kiralık durumda. Garaja dönmeden tekrar kiralanamaz!");
            }

            await _rentalRepository.AddAsync(rental);
        }

        public async Task DeleteAsync(Rental rental)
        {
            await _rentalRepository.DeleteAsync(rental);
        }

        public async Task<List<Rental>> GetAllAsync()
        {
            return await _rentalRepository.GetAllAsync();
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            return await _rentalRepository.GetAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Rental rental)
        {
            await _rentalRepository.UpdateAsync(rental);
        }
    }
}
