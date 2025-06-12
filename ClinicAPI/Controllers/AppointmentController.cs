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
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Admin sees all, user sees only their own
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointments()
        {
            var userEmail = User.Identity?.Name; // populated from ClaimTypes.Name
            var isAdmin = User.IsInRole("Admin");

            var query = _context.Appointments
                .Include(a => a.Doctor)
                .AsQueryable();

            if (!isAdmin && !string.IsNullOrEmpty(userEmail))
            {
                query = query.Where(a => a.PatientName == userEmail);
            }

            var result = await query
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.PatientName,
                    AppointmentDate = a.AppointmentDate,
                    DoctorId = a.DoctorId,
                    Notes = a.Notes,
                    DoctorName = a.Doctor.FullName
                })
                .ToListAsync();

            return Ok(result);
        }

        // ✅ Only logged-in users can create appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentDto dto)
        {
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userEmail))
                return Unauthorized("Missing user identity.");

            if (dto.DoctorId <= 0)
                return BadRequest("Valid doctor ID is required.");

            if (dto.AppointmentDate <= DateTime.Now)
                return BadRequest("Appointment must be in the future.");

            var appointment = new Appointment
            {
                PatientName = userEmail,
                AppointmentDate = dto.AppointmentDate,
                DoctorId = dto.DoctorId,
                Notes = dto.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllAppointments), new { id = appointment.Id }, appointment);
        }

        // ✅ Admin can update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment updated)
        {
            if (id != updated.Id)
                return BadRequest("ID mismatch");

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.AppointmentDate = updated.AppointmentDate;
            appointment.DoctorId = updated.DoctorId;
            appointment.Notes = updated.Notes;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Admin can delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
