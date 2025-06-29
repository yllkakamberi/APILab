using System;

namespace ClinicAPI.Models
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        public string? PatientName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }

        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }

        public string? Notes { get; set; }
    }
}
