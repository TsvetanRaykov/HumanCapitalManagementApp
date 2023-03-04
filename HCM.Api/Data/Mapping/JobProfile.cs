using AutoMapper;
using HCM.Api.Data.Models;
using HCM.Shared.Data.DTO;

namespace HCM.Api.Data.Mapping;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<JobDto, Job>()
            .ForMember(dest => dest.Employees, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore());

        CreateMap<Job, JobDto>();
        CreateMap<Job, Job>();
    }
}