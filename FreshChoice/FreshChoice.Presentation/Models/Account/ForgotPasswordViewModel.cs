using System.ComponentModel.DataAnnotations;

namespace FreshChoice.Presentation.Models.Account;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}