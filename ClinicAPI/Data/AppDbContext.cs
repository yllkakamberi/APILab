using ClinicAPI.Domain.Entities;
using ClinicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Review> Reviews { get; set; } // ✅ Added Reviews

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Department -> Doctors (1:N)
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Doctors)
                .WithOne(doc => doc.Department)
                .HasForeignKey(doc => doc.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Department -> Services (1:N)
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Services)
                .WithOne(s => s.Department)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment -> Doctor (N:1)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment -> Service (N:1)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment -> Reviews (1:N)
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.Reviews)
                .WithOne(r => r.Appointment)
                .HasForeignKey(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Fix decimal precision warning for Service.Price
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
