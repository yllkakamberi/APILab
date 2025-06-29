// DomainApi/DTOs/ReviewDto.cs
namespace ClinicAPI.DomainApi.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
