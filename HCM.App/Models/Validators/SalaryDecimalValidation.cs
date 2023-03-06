using System.ComponentModel.DataAnnotations;

namespace HCM.App.Models.Validators;

public class SalaryDecimalValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var strSalary = value?.ToString();

        if (decimal.TryParse(strSalary, out _)) return null;
       
        return new ValidationResult(ErrorMessage);

    }
}