using System.ComponentModel.DataAnnotations;

namespace HCM.App.Models.Validators;

public class SalaryPositiveValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var strSalary = value?.ToString();

        if (!decimal.TryParse(strSalary, out var decSalary)) return null;

        if (decSalary <= 0) return new ValidationResult(ErrorMessage);

        return null;
    }
}