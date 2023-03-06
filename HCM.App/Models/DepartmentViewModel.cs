using System.ComponentModel.DataAnnotations;

namespace HCM.App.Models;

public class DepartmentViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Department Name must be at least 3 characters long.")]
    public string Name { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Department Address must be at least 3 characters long.")]
    public string Address { get; set; }

    public string? Description { get; set; }
}