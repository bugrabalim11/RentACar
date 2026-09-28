namespace RentACar.Core.Entities.Concrete
{
    public class User : BaseEntity, IEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        // byte dizisi yani byte[] formatında olacak!
        // Çünkü kriptografi algoritmaları metinlerle değil, baytlarla - 0 ve 1'lerle - çalışır.
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    }
}
