using ClinicAPI.Data;
using ClinicAPI.Domain.Entities;
using ClinicAPI.DomainApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.RestAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.Service)
                .Select(r => new
                {
                    r.Id,
                    r.AppointmentId,
                    r.Comment,
                    r.Rating,
                    PatientName = r.Appointment.PatientName,
                    DoctorName = r.Appointment.Doctor.FullName,
                    ServiceName = r.Appointment.Service.Name
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.Service)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return NotFound();

            var dto = new
            {
                review.Id,
                review.AppointmentId,
                review.Comment,
                review.Rating,
                PatientName = review.Appointment.PatientName,
                DoctorName = review.Appointment.Doctor.FullName,
                ServiceName = review.Appointment.Service.Name
            };

            return Ok(dto);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.Service)
                .Where(r => r.AppointmentId == appointmentId)
                .Select(r => new
                {
                    r.Id,
                    r.AppointmentId,
                    r.Comment,
                    r.Rating,
                    PatientName = r.Appointment.PatientName,
                    DoctorName = r.Appointment.Doctor.FullName,
                    ServiceName = r.Appointment.Service.Name
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewDto dto)
        {
            var review = new Review
            {
                AppointmentId = dto.AppointmentId,
                Comment = dto.Comment,
                Rating = dto.Rating
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            dto.Id = review.Id;
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
