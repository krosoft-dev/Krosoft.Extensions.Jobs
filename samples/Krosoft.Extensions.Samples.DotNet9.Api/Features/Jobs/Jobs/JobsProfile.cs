using AutoMapper;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Mapping.Extensions;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;

public class JobsProfile : Profile
{
    public JobsProfile()
    {
        CreateMap<IJobAutomatiqueSetting, JobDto>()
            .ForMember(dest => dest.Identifiant, o => o.MapFrom(src => src.Identifiant))
            .ForMember(dest => dest.CronExpression, o => o.MapFrom(src => src.CronExpression))
            .ForMember(dest => dest.TypeCode, o => o.MapFrom(src => src.Type))
            .ForAllOtherMembers(m => m.Ignore());

        CreateMap<CronJob, JobDto>()
            .ForMember(dest => dest.Identifiant, o => o.MapFrom(src => src.Identifiant))
            .ForMember(dest => dest.ProchaineExecutionDate, o => o.MapFrom(src => src.ProchaineExecutionDate))
            .ForMember(dest => dest.DerniereExecutionDate, o => o.MapFrom(src => src.DerniereExecutionDate))
            .ForMember(dest => dest.DerniereExecutionStatut, o => o.MapFrom(src => src.DerniereExecutionStatut))
            .ForMember(dest => dest.CreationDate, o => o.MapFrom(src => src.CreationDate))
            .ForAllOtherMembers(m => m.Ignore());
    }
}