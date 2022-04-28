using TeleDoc.DAL.Entities;

namespace TeleDoc.API.Area.Patients.Models;

public class Patient : ApplicationUser
{
    public new string? Disease { get; set; }
    public IList<Prescription>? Prescriptions { get; set; }

}
