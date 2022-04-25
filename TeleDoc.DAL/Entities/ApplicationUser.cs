using Microsoft.AspNetCore.Identity;

namespace TeleDoc.DAL.Entities;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Disease { get; set; }
    public string? Address { get; set; }
    
    
    
    
}