using TeleDoc.API.Area.Patients.Models;

namespace TeleDoc.API.Dtos.Patients;

public class PatientDetailsDto
{
    public string? UserName { get; set; }
     public string? Name { get; set; }
     public string? Email { get; set; }
     public string? Gender { get; set; }
     public DateTime DateOfBirth { get; set; }
     public string? Address { get; set; }
     public string? PhoneNumber { get; set; }
     public string? Disease { get; set; }
     
     public IList<Prescription>? Prescriptions { get; set; }
}