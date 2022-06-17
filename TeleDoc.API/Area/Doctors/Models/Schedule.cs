using System.ComponentModel.DataAnnotations;
using DayOfWeek = TeleDoc.API.Area.Admins.Enums.DayOfWeek;

namespace TeleDoc.API.Area.Doctors.Models;

public class Schedule
{
    [Key]
    public int ScheduleId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    private int PatientCount { get; set; }
    public int PatientLimit { get; set; }
    
    public List<BookingSchedule>? Patients { get; set; }
    
}