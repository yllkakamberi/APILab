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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PatientName))
                return BadRequest("Patient name is required.");
            if (dto.DoctorId <= 0)
                return BadRequest("Valid doctor ID is required.");
            if (dto.AppointmentDate < DateTime.Now)
                return BadRequest("Appointment date must be in the future.");

            var appointment = new Appointment
            {
                PatientName = dto.PatientName,
                AppointmentDate = dto.AppointmentDate,
                DoctorId = dto.DoctorId,
                Notes = dto.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllAppointments), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment updated)
        {
            if (id != updated.Id)
                return BadRequest("ID mismatch");

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(updated.PatientName))
                return BadRequest("Patient name is required.");

            appointment.PatientName = updated.PatientName;
            appointment.AppointmentDate = updated.AppointmentDate;
            appointment.DoctorId = updated.DoctorId;
            appointment.Notes = updated.Notes;

            await _context.SaveChangesAsync();
            return NoContent();
        }

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
