using System.ComponentModel.DataAnnotations;

namespace TeleDoc.API.Area.Doctors.Models;

public class Schedule
{
    public int ScheduleId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public Doctor? Doctor { get; set; }
}