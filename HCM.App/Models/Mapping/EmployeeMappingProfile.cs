using System.Globalization;
using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public static class EmployeeMappingProfile
{
    public static EmployeeDto ToEmployeeDto(this EmployeeViewModel vm)
    {
        return new EmployeeDto
        {
            DepartmentId = vm.DepartmentId,
            Email = vm.Email,
            FirstName = vm.FirstName,
            HireDate = vm.HireDate,
            Id = vm.Id,
            JobId = vm.JobId,
            LastName = vm.LastName,
            PhoneNumber = vm.PhoneNumber,
            Salary = decimal.Parse(vm.Salary)
        };
    }

    public static EmployeeViewModel ToEmployeeViewModel(this EmployeeDto? vm)
    {
        return new EmployeeViewModel()
        {
            DepartmentId = vm.DepartmentId,
            Email = vm.Email,
            FirstName = vm.FirstName,
            HireDate = vm.HireDate,
            Id = vm.Id,
            JobId = vm.JobId,
            LastName = vm.LastName,
            PhoneNumber = vm.PhoneNumber,
            Salary = vm.Salary.ToString(CultureInfo.InvariantCulture)
        };
    }


}