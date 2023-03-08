using HCM.Shared.Data.DTO;
using System.Globalization;

namespace HCM.App.Models.Mapping;

public static class JobMappingProfile
{
    public static JobViewModel ToJobViewModel(this JobDto? dto)
    {
        return new JobViewModel
        {
            Title = dto.Title,
            Description = dto.Description,
            Id = dto.Id,
            MaxSalaryString = dto.MaxSalary.ToString("F", CultureInfo.InvariantCulture),
            MinSalaryString = dto.MinSalary.ToString("F", CultureInfo.InvariantCulture)
        };
    }

    public static JobDto ToJobDto(this JobViewModel? vm)
    {
        return new JobDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Description = vm.Description,
            MaxSalary = decimal.Parse(vm.MaxSalaryString),
            MinSalary = decimal.Parse(vm.MinSalaryString)
        };
    }
}