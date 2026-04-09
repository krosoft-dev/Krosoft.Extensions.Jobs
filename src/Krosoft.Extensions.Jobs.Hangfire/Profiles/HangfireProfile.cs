using AutoMapper;
using Hangfire.Storage;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Mapping.Extensions;

namespace Krosoft.Extensions.Jobs.Hangfire.Profiles;

public class HangfireProfile : Profile
{
    public HangfireProfile()
    {
        CreateMap<RecurringJobDto, CronJob>()
            .ForMember(dest => dest.Identifiant, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.CronExpression, o => o.MapFrom(src => src.Cron))
            .ForMember(dest => dest.ProchaineExecutionDate, o => o.MapFrom(src => src.NextExecution))
            .ForMember(dest => dest.DerniereExecutionStatut, o => o.MapFrom(src => src.LastJobState))
            .ForMember(dest => dest.DerniereExecutionDate, o => o.MapFrom(src => src.LastExecution))
            .ForMember(dest => dest.QueueName, o => o.MapFrom(src => src.Queue))
            .ForMember(dest => dest.CreationDate, o => o.MapFrom(src => src.CreatedAt))
            .ForAllOtherMembers(m => m.Ignore());
    }
}