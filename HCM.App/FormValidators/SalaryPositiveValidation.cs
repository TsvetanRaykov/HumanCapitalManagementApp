namespace HCM.App.FormValidators;

using System.ComponentModel.DataAnnotations;

public class SalaryPositiveValidation : ValidationAttribute
{
    private string ErrorMessage { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var strSalary = value?.ToString();

        if (!decimal.TryParse(strSalary, out var decSalary)) return null;

        if (decSalary <= 0) return new ValidationResult(ErrorMessage);

        return null;
    }
}