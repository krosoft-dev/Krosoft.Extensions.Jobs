using AutoMapper;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<JobAmqpSettings, JobAutomatiqueSetting>()
            .ForMember(dest => dest.Identifiant, o => o.MapFrom(src => src.Identifiant))
            .ForMember(dest => dest.CronExpression, o => o.MapFrom(src => src.CronExpression))
            .ForMember(dest => dest.Type, o => o.MapFrom(src => JobTypeCode.Amqp))
            .ForMember(dest => dest.QueueName, o => o.MapFrom(src => Constantes.QueuesKeys.Default))
            .ForAllOtherMembers(m => m.Ignore());
    }
}