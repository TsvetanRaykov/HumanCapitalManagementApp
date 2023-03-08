using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public static class DepartmentMappingProfile
{
    public static DepartmentViewModel ToDepartmentViewModel(this DepartmentDto dto)
    {
        return new DepartmentViewModel
        {
            Address = dto.Address,
            Description = dto.Description,
            Id = dto.Id,
            Name = dto.Name
        };
    }

    public static DepartmentDto ToDepartmentDto(this DepartmentViewModel? vm)
    {
        return new DepartmentDto
        {
            Address = vm.Address,
            Description = vm.Description,
            Id = vm.Id,
            Name = vm.Name
        };
    }

}