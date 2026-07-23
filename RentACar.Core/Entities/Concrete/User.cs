using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Entities.Concrete
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        // byte dizisi yani byte[] formatında olacak!
        // Çünkü kriptografi algoritmaları metinlerle değil, baytlarla - 0 ve 1'lerle - çalışır.
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public bool Status { get; set; } = true;
    }
}
