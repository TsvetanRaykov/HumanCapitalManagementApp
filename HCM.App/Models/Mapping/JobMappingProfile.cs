﻿using System.Globalization;
using AutoMapper;
using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public class JobMappingProfile : Profile
{
    public JobMappingProfile()
    {
        CreateMap<JobViewModel, JobDto>()
            .ForMember(dest => dest.MinSalary, opt => opt.MapFrom(src => decimal.Parse(src.MinSalaryString)))
            .ForMember(dest => dest.MaxSalary, opt => opt.MapFrom(src => decimal.Parse(src.MaxSalaryString)));

        CreateMap<JobDto, JobViewModel>()
            .ForMember(dest => dest.MinSalaryString,
                opt => opt.MapFrom(src => src.MinSalary.ToString("F", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.MaxSalaryString,
                opt => opt.MapFrom(src => src.MaxSalary.ToString("F", CultureInfo.InvariantCulture)));
    }
}