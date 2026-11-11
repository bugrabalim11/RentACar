using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.Signatures;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    /// <summary>
    /// Entity Framework Core'un veritabanı ile uygulama arasındaki bağlantıyı sağlayan bağlam (Context) sınıfıdır.
    /// Soft Delete (Silinmiş gibi gösterme), Audit Logging (Tarih damgaları) ve Global Sorgu Filtrelerini yönetir.
    /// </summary>
    public class RentACarContext : DbContext
    {
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
        public DbSet<CarMaintenance> CarMaintenances { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted when entry.Entity is not IHardDeletable:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedDate = DateTime.UtcNow;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İki ofis alanı (PickUp, DropOff) için kaskad (Cascade) silme davranışlarını kapatıyoruz
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.PickUpOffice)
                .WithMany()
                .HasForeignKey(r => r.PickUpOfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.DropOffOffice)
                .WithMany()
                .HasForeignKey(r => r.DropOffOfficeId)
                .OnDelete(DeleteBehavior.Restrict);



            // Soft Delete (Global Query Filters) - Silinmiş kayıtları sorgulardan otomatik gizler.
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
            modelBuilder.Entity<CarMaintenance>().HasQueryFilter(cm => cm.IsDeleted == false);
            modelBuilder.Entity<Payment>().HasQueryFilter(p => p.IsDeleted == false);


            // --- DATA SEEDING (VERİ TOHUMLAMA) ALANI ---
            byte[] passwordSalt = System.Text.Encoding.UTF8.GetBytes("RentACar-Secret-Static-Salt-Key-123");
            byte[] passwordHash;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Admin123"));
            }

            DateTime staticDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@rentacar.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDate = staticDate
            });

            modelBuilder.Entity<OperationClaim>().HasData(
                new OperationClaim { Id = 1, Name = "admin", CreatedDate = staticDate },
                new OperationClaim { Id = 2, Name = "user", CreatedDate = staticDate }
            );

            modelBuilder.Entity<UserOperationClaim>().HasData(new UserOperationClaim
            {
                Id = 1,
                UserId = 1,
                OperationClaimId = 1,
                IsDeleted = false,
                CreatedDate = staticDate
            });
        }
    }
}
