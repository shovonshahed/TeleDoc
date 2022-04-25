using System.ComponentModel.DataAnnotations;

namespace TeleDoc.API.Models.Account;

public class RegisterViewModel
{
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? Email { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Password must be matched")]
    public string? ConfirmPassword { get; set; }

}