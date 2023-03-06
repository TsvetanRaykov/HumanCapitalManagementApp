using AutoMapper;
using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<DepartmentViewModel, DepartmentDto>();
        CreateMap<DepartmentDto, DepartmentViewModel>();
    }
}