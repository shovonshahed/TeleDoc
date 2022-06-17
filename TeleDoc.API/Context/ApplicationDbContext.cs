using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Models;

namespace TeleDoc.API.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    
    

    public DbSet<ApplicationUser>? ApplicationUser { get; set; }
    public DbSet<Patient>? Patient { get; set; }
    public DbSet<Doctor>? Doctor { get; set; }
    public DbSet<Schedule>? Schedules { get; set; }
    public DbSet<BookingSchedule>? Booking { get; set; }
    
    
}