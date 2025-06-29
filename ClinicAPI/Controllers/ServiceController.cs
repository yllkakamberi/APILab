using ClinicAPI.Data;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Service
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAllServices()
        {
            var services = await _context.Services
                .Include(s => s.Department)
                .ToListAsync();

            var dtoList = new List<ServiceDto>();
            foreach (var s in services)
            {
                dtoList.Add(new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    DepartmentId = s.DepartmentId
                });
            }

            return Ok(dtoList);
        }

        // GET: api/Service/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            return Ok(new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DepartmentId = service.DepartmentId
            });
        }

        // GET: api/Service/by-department/{departmentId}
        [HttpGet("by-department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServicesByDepartment(int departmentId)
        {
            var services = await _context.Services
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync();

            var dtoList = new List<ServiceDto>();
            foreach (var s in services)
            {
                dtoList.Add(new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    DepartmentId = s.DepartmentId
                });
            }

            return Ok(dtoList);
        }

        // POST: api/Service
        [HttpPost]
        public async Task<ActionResult<ServiceDto>> CreateService(ServiceDto dto)
        {
            var service = new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DepartmentId = dto.DepartmentId
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            dto.Id = service.Id;
            return Ok(dto);
        }

        // PUT: api/Service/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, ServiceDto dto)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            service.Name = dto.Name;
            service.Description = dto.Description;
            service.Price = dto.Price;
            service.DepartmentId = dto.DepartmentId;

            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        // DELETE: api/Service/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
