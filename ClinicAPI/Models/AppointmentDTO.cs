using System;

namespace ClinicAPI.Models
{
    public class AppointmentDto
    {
        public int Id { get; set; } // ✅ Required for editing/updating

        // ✅ Make optional so frontend doesn't have to send it
        public string? PatientName { get; set; }

        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public string? Notes { get; set; }

        // ✅ Optional: used for displaying doctor's full name
        public string? DoctorName { get; set; }
    }
}
