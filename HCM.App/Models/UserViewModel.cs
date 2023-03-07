namespace HCM.App.Models;

using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    public string? Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    [Required]
    public string? Role { get; set; }

    public string? DisplayRole => Role == null ? null : char.ToUpper(Role[0]) + Role[1..];

    [ValidateComplexType] public PasswordViewModel? Passwords { get; set; }
}