using ClinicAPI.Models;

namespace ClinicAPI.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }

        public Appointment? Appointment { get; set; }
    }
}
