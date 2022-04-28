using TeleDoc.DAL.Entities;

namespace TeleDoc.API.Area.Doctors.Models;

public class Doctor : ApplicationUser
{
    public new string? Speciality { get; set; }
    public new string? College { get; set; }
    public new string? CertificateUrl { get; set; }
    
    
    
}