using System.ComponentModel.DataAnnotations;

namespace FreshChoice.Presentation.Models.Account;

public class ResetPasswordViewModel
{
    [Required(
        ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    [Required]
    public string? Token { get; set; }
    [Required(
        ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
    [Compare(
        nameof(Password),
        ErrorMessage = "Password is different.")]
    public string? ConfirmPassword { get; set; }
}