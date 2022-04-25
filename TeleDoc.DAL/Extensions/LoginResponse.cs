using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Enums;

namespace TeleDoc.DAL.Extensions;

public class LoginResponse
{
    public ApplicationUser? User { get; set; }
    public string? Token { get; set; }
    public ResponseStatus Status { get; set; }
    
}