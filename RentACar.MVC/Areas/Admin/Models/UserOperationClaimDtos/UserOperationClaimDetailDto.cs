namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos
{
    public class UserOperationClaimDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = null!;  
        public int OperationClaimId { get; set; }
        public string ClaimName { get; set; } = null!;
    }
}
