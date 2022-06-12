using System.ComponentModel.DataAnnotations;

namespace TeleDoc.API.Models.Account;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
}