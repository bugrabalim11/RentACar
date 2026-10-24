using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.OperationClaimDtos
{
    public class OperationClaimUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
