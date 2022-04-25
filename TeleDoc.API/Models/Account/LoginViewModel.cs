using System.ComponentModel.DataAnnotations;

namespace TeleDoc.API.Models.Account;

public class LoginViewModel
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}