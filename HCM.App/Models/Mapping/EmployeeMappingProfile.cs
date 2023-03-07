using AutoMapper;
using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<EmployeeDto, EmployeeViewModel>()
            .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate.ToLocalTime()));
        CreateMap<EmployeeViewModel, EmployeeDto>();
    }
}