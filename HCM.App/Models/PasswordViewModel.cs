namespace HCM.App.Models;

using System.ComponentModel.DataAnnotations;

public class PasswordViewModel
{
    [Required]
    public string? Password { get; set; }

    [Display(Name = "Confirm new password")]
    [Compare(nameof(Password), ErrorMessage = "The new password and confirm passwords do not match.")]
    public string ConfirmPassword { get; set; }
}