using ClinicAPI.Data;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAllDoctors()
        {
            return await _context.Doctors.Include(d => d.Department).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctorById(int id)
        {
            var doctor = await _context.Doctors.Include(d => d.Department).FirstOrDefaultAsync(d => d.Id == id);
            return doctor == null ? NotFound() : Ok(doctor);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Doctor>>> SearchDoctors(string? name, string? department)
        {
            var query = _context.Doctors.Include(d => d.Department).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.FullName.Contains(name));

            if (!string.IsNullOrEmpty(department))
                query = query.Where(d => d.Department.Name.Contains(department));

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id)
                return BadRequest();

            var existing = await _context.Doctors.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.FullName = doctor.FullName;
            existing.Email = doctor.Email;
            existing.DepartmentId = doctor.DepartmentId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
