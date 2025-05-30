using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClinicAPI.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
