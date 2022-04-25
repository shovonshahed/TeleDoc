using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.DAL.Entities;

namespace TeleDoc.API.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser>? ApplicationUser { get; set; }
    public DbSet<Patient>? Patient { get; set; }
    public DbSet<Doctor>? Doctor { get; set; }
    
    
}