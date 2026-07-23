using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities.Concrete;
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
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<User> Users { get; set; }

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



            // ----------------------------------------------------------------------
            // 2. GLOBAL QUERY FILTERS (KÜRESEL SORGULAMA FİLTRELERİ) - SOFT DELETE MİMARİSİ
            // ----------------------------------------------------------------------
            // KURAL: Sistemde herhangi bir listeleme (Get, GetAll vs.) işlemi yapıldığında,
            // Entity Framework arka planda otomatik olarak "Status == true" (Aktif olanlar) şartını SQL sorgusuna ekler.
            // Bu sayede "Silinmiş" (Status = false) veriler sistemde asla listelenmez.

            modelBuilder.Entity<User>().HasQueryFilter(u => u.Status == true);
            modelBuilder.Entity<UserOperationClaim>().HasQueryFilter(uoc => uoc.Status == true);
            modelBuilder.Entity<OperationClaim>().HasQueryFilter(oc => oc.Status == true);
            modelBuilder.Entity<Brand>().HasQueryFilter(b => b.Status == true);
            modelBuilder.Entity<Car>().HasQueryFilter(c => c.Status == true);
            modelBuilder.Entity<Color>().HasQueryFilter(c => c.Status == true);
            modelBuilder.Entity<ContactInfo>().HasQueryFilter(ci => ci.Status == true);
            modelBuilder.Entity<ContactMessage>().HasQueryFilter(cm => cm.Status == true);
            modelBuilder.Entity<Customer>().HasQueryFilter(c => c.Status == true);
            modelBuilder.Entity<Office>().HasQueryFilter(o => o.Status == true);
            modelBuilder.Entity<Rental>().HasQueryFilter(r => r.Status == true);
        }
    }
}
