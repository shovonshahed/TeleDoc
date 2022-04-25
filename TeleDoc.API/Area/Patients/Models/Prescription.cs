using System.ComponentModel.DataAnnotations;
using TeleDoc.API.Area.Admins.Models;
using TeleDoc.API.Area.Doctors.Models;

namespace TeleDoc.API.Area.Patients.Models;

public class Prescription
{
    [Key]
    public int Id { get; set; }

    private DateTime IssueDate { get; set; }
    public IList<Medicine>? Medicines { get; set; }
    
    public Doctor? Doctor { get; set; }
    

    public Prescription()
    {
        IssueDate = DateTime.Now;
    }
}