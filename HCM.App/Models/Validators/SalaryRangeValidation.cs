using System.ComponentModel.DataAnnotations;

namespace HCM.App.Models.Validators;

public class SalaryRangeValidation : ValidationAttribute
{
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var strSalary = value?.ToString();

        if (strSalary != null && decimal.TryParse(strSalary, out var salary))
        {
            if (salary >= MinAmount && salary <= MaxAmount) return null;
        }

        return new ValidationResult(ErrorMessage);
    }
}