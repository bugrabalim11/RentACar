namespace RentACar.MVC.Areas.Admin.Models.OfficeDtos
{
    public class OfficeResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
