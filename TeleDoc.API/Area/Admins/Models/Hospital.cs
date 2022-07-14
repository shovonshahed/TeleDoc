using TeleDoc.API.Models;

namespace TeleDoc.API.Area.Admins.Models;

public class Hospital
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Details { get; set; }
    public MapLocation? Location { get; set; }
}