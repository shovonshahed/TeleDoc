using TeleDoc.API.Enums;

namespace TeleDoc.API.Extensions;

public class LoginResponse
{
    public Object? Data { get; set; }
    public string? Token { get; set; }
    public ResponseStatus Status { get; set; }
    
}