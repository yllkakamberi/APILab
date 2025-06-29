using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

        public ICollection<Service> Services { get; set; } = new List<Service>(); // ✅ Added
    }
}
