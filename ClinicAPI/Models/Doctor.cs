using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        [JsonIgnore]
        public Department Department { get; set; }

        [JsonIgnore]
        public ICollection<Appointment> Appointments { get; set; }
    }
}
