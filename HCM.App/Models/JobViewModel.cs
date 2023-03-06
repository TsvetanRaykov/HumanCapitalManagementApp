using System.ComponentModel.DataAnnotations;
using HCM.App.Models.Validators;

namespace HCM.App.Models;

public class JobViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Job Title is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Job Title must be at least 3 characters long.")]
    public string Title { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Minimum Salary is required.")]
    [SalaryDecimalValidation(ErrorMessage = "Invalid Minimum Salary format. Please use positive decimal number with two decimal places.")]
    [SalaryPositiveValidation(ErrorMessage = "Minimum Salary should not be less than 0.")]
    [SalaryRangeValidation(MinAmount = 1, MaxAmount = 1000000, ErrorMessage = "Minimum Salary should be in range 1 - 1000000.")]
    public string MinSalaryString { get; set; }

    [Required(ErrorMessage = "Maximum Salary is required.")]
    [SalaryDecimalValidation(ErrorMessage = "Invalid Maximum Salary format. Please use positive decimal number with two decimal places.")]
    [SalaryPositiveValidation(ErrorMessage = "Maximum Salary should not be less than 0.")]
    [SalaryRangeValidation(MinAmount = 1, MaxAmount = 1000000, ErrorMessage = "Maximum Salary should be in range 1 - 1000000.")]
    public string MaxSalaryString { get; set; }
}