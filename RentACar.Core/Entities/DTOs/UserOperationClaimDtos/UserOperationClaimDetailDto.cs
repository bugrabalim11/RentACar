using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Entities.DTOs.UserOperationClaimDtos
{
    // Unutma: Bu bir veritabanı tablosu değil (IEntity), bu bir form/tabak (IDto)
    public class UserOperationClaimDetailDto : IDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = null!;  // FirstName ve LastName'i birleştirip buraya atacağız
        public int OperationClaimId { get; set; }
        public string ClaimName { get; set; } = null!;  // Rütbenin ekranda yazacak olan adı (Örn: "Admin")
    }
}
