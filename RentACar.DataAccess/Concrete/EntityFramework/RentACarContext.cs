using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities.Concrete;
using RentACar.Entities.Concrete;

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
        public DbSet<CarImage> CarImages { get; set; }

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
            // Entity Framework arka planda otomatik olarak "IsDeleted == false" (Silinmemiş olanlar) şartını SQL sorgusuna ekler.
            // Bu sayede "Silinmiş" (IsDeleted = true) veriler sistemde asla listelenmez.

            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsDeleted == false);
            modelBuilder.Entity<UserOperationClaim>().HasQueryFilter(uoc => uoc.IsDeleted == false);
            modelBuilder.Entity<OperationClaim>().HasQueryFilter(oc => oc.IsDeleted == false);
            modelBuilder.Entity<Brand>().HasQueryFilter(b => b.IsDeleted == false);
            modelBuilder.Entity<Car>().HasQueryFilter(c => c.IsDeleted == false);
            modelBuilder.Entity<Color>().HasQueryFilter(c => c.IsDeleted == false);
            modelBuilder.Entity<ContactInfo>().HasQueryFilter(ci => ci.IsDeleted == false);
            modelBuilder.Entity<ContactMessage>().HasQueryFilter(cm => cm.IsDeleted == false);
            modelBuilder.Entity<Customer>().HasQueryFilter(c => c.IsDeleted == false);
            modelBuilder.Entity<Office>().HasQueryFilter(o => o.IsDeleted == false);
            modelBuilder.Entity<Rental>().HasQueryFilter(r => r.IsDeleted == false);
            modelBuilder.Entity<CarImage>().HasQueryFilter(ci => ci.IsDeleted == false);
        }
    }
}
