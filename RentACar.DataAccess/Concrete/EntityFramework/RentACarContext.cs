using Microsoft.EntityFrameworkCore;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class RentACarContext : DbContext
    {
        // İŞTE EKSİK OLAN HAYATİ KOD BURASI: Şifreyi alıp ana motora (base) iletiyor
        public RentACarContext(DbContextOptions<RentACarContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // RENTAL VE OFFICE ARASINDAKİ ÇİFT İLİŞKİ KURALI
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.PickUpOffice)            // Bir kiralamanın bir Alış Ofisi vardır
                .WithMany()                             // Bir ofisin birden çok kiralaması olabilir
                .HasForeignKey(r => r.PickUpOfficeId)   // Kancamız budur
                .OnDelete(DeleteBehavior.Restrict);     // KURAL: Ofis silinirse, kiralama fişini SİLME! Sistemi koru.

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.DropOffOffice)
                .WithMany()
                .HasForeignKey(r => r.DropOffOfficeId)
                .OnDelete(DeleteBehavior.Restrict);    // KURAL: Teslim ofisi silinirse, kiralama fişini SİLME!
        }
    }
}
