using AutoMapper;
using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<DepartmentViewModel, DepartmentDto>();
        CreateMap<DepartmentDto, DepartmentViewModel>();
    }
}