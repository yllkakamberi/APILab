using ClinicAPI.Data;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<object>>> GetAllDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.Email,
                    d.DepartmentId,
                    DepartmentName = d.Department.Name
                })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDoctorById(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.Email,
                    d.DepartmentId,
                    DepartmentName = d.Department.Name
                })
                .FirstOrDefaultAsync();

            return doctor == null ? NotFound() : Ok(doctor);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchDoctors(string? name, int? departmentId)
        {
            var query = _context.Doctors.Include(d => d.Department).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.FullName.Contains(name));

            if (departmentId.HasValue)
                query = query.Where(d => d.DepartmentId == departmentId.Value);

            var result = await query
                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.Email,
                    d.DepartmentId,
                    DepartmentName = d.Department.Name
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> AddDoctor([FromBody] DoctorDto dto)
        {
            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            doctor.FullName = dto.FullName;
            doctor.Email = dto.Email;
            doctor.DepartmentId = dto.DepartmentId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
