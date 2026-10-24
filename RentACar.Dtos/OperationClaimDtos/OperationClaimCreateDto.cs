using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.OperationClaimDtos
{
    public class OperationClaimCreateDto : IDto
    {
        public string Name { get; set; } = null!;
    }
}
