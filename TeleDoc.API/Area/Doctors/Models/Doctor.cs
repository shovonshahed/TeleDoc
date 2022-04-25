using TeleDoc.DAL.Entities;

namespace TeleDoc.API.Area.Doctors.Models;

public class Doctor : ApplicationUser
{
    public string? Speciality { get; set; }
    public string? College { get; set; }
    public string? CertificateUrl { get; set; }
    
    
    
}