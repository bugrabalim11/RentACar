namespace RentACar.MVC.Areas.Admin.Models.UserDtos
{
    public class UserResultForAdminDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
