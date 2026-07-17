using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RentalAddDto> _addValidator;
        private readonly IValidator<RentalUpdateDto> _updateValidator;
        public RentalManager(IRentalRepository rentalRepository, IMapper mapper, IValidator<RentalAddDto> addValidator, IValidator<RentalUpdateDto> updateValidator)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(RentalAddDto rentalAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(rentalAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // --- POSTGRESQL ZAMAN DİLİMİ KURALI (UTC) ---
            // PostgreSQL, saat dilimi belirtilmemiş (Kind=Unspecified) tarihleri kabul etmez.
            // Frontend veya Swagger'dan gelen saf tarihleri veritabanı deposuna göndermeden önce,
            // Gümrük kurallarına uymak adına Evrensel Saate (UTC) dönüştürerek standartlaştırıyoruz.
            rentalAddDto.RentDate = rentalAddDto.RentDate.ToUniversalTime();
            if (rentalAddDto.ReturnDate.HasValue)
            {
                rentalAddDto.ReturnDate = rentalAddDto.ReturnDate.Value.ToUniversalTime();
            }

            var rental = _mapper.Map<Rental>(rentalAddDto);
            await _rentalRepository.AddAsync(rental);
            return new SuccessResult("Araç kiralama başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == id);
            if (existingRental == null)
            {
                return new ErrorResult("Silinecek araç kiralama bulunamadı.");
            }

            await _rentalRepository.DeleteAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla silindi.");
        }

        public async Task<IDataResult<List<RentalListDto>>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetRentalsWithDetailsAsync();
            var rentalsListDtos = _mapper.Map<List<RentalListDto>>(rentals);
            return new SuccessDataResult<List<RentalListDto>>(rentalsListDtos, "Kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<RentalListDto>> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(id);
            if (rental == null)
            {
                return new ErrorDataResult<RentalListDto>("Aranan araç kiralama bulunamadı.");
            }

            var rentalListDto = _mapper.Map<RentalListDto>(rental);
            return new SuccessDataResult<RentalListDto>(rentalListDto, "Araç kiralama detayı getirildi.");
        }

        public async Task<IResult> UpdateAsync(RentalUpdateDto rentalUpdateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(rentalUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingRental = await _rentalRepository.GetAsync(x => x.Id == rentalUpdateDto.Id);
            if (existingRental == null)
            {
                return new ErrorResult("Güncellenecek araç kiralama bulunamadı.");
            }

            rentalUpdateDto.RentDate = rentalUpdateDto.RentDate.ToUniversalTime();
            if (rentalUpdateDto.ReturnDate.HasValue)
            {
                rentalUpdateDto.ReturnDate = rentalUpdateDto.ReturnDate.Value.ToUniversalTime();
            }

            _mapper.Map(rentalUpdateDto, existingRental);
            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla güncellendi.");
        }
    }
}
