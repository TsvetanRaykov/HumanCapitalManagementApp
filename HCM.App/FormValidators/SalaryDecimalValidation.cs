using System.ComponentModel.DataAnnotations;

namespace HCM.App.FormValidators;

public class SalaryDecimalValidation : ValidationAttribute
{
    private string ErrorMessage { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var strSalary = value?.ToString();

        if (!decimal.TryParse(strSalary, out var decSalary))
        {
            return new ValidationResult(ErrorMessage);
        }

        return null;
    }
}