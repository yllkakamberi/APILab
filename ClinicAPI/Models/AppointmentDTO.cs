using System;

namespace ClinicAPI.Models
{
    public class AppointmentDto
    {
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public string Notes { get; set; }
    }
}
