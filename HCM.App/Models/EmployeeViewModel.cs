using System.ComponentModel.DataAnnotations;

namespace HCM.App.Models;

using Validators;
using HCM.Shared.Data.DTO;

public class EmployeeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(30, MinimumLength = 1)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(30, MinimumLength = 1)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Hire Date is required.")]
    public DateTime HireDate { get; set; }

    [Required]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [StringLength(20, MinimumLength = 1)]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Salary is required.")]
    [SalaryDecimalValidation(ErrorMessage = "Invalid Salary format. Please use positive decimal number with two decimal places.")]
    [SalaryPositiveValidation(ErrorMessage = "Salary should not be less than 0.")]
    public string Salary { get; set; }

    [Required(ErrorMessage = "Job is required.")]
    public int JobId { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    public int DepartmentId { get; set; }

    public JobDto? Job { get; set; }

    public DepartmentDto? Department { get; set; }
}