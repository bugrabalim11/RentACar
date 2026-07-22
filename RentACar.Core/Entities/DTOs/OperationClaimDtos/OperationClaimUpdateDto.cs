using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Entities.DTOs.OperationClaimDtos
{
    public class OperationClaimUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
