namespace RentACar.Core.Entities.DTOs.OperationClaimDtos
{
    public class OperationClaimUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
