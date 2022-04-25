namespace TeleDoc.API.Area.Admins.Models;

public class Medicine
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Unit { get; set; } // 20mg
    public MedicinePeriod? Period { get; set; }
    
}