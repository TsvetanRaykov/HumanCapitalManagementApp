using AutoMapper;

namespace HCM.Api.Data.Mapping;
using HCM.Shared.Data.DTO;
using Models;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Job, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore());

        CreateMap<Employee, EmployeeDto>();

    }
}