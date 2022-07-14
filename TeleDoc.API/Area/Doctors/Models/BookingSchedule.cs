using System.ComponentModel.DataAnnotations;
using TeleDoc.API.Models;

namespace TeleDoc.API.Area.Doctors.Models;

public class BookingSchedule
{
    public int Id { get; set; }
    public string? PatientEmail { get; set; }
    public Schedule? Schedules { get; set; }
}   