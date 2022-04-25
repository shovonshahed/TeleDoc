namespace TeleDoc.API.Area.Admins.Models;

public class MedicinePeriod
{
    public int Id { get; set; }
    
    public bool? Morning { get; set; }
    public bool? AfterNoon { get; set; }
    public bool? Night { get; set; }
    
}