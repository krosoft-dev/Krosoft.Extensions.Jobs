using AutoMapper;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Mapping.Extensions;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;

public class JobsQueryHandler : IRequestHandler<JobsQuery, IEnumerable<JobDto>>
{
    private readonly IJobManager _jobManager;
    private readonly IJobSettingStore _jobSettingStore;
    private readonly ILogger<JobsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public JobsQueryHandler(ILogger<JobsQueryHandler> logger,
                            IMapper mapper,
                            IJobManager jobManager,
                            IJobSettingStore jobSettingStore)
    {
        _logger = logger;
        _mapper = mapper;
        _jobManager = jobManager;
        _jobSettingStore = jobSettingStore;
    }

    public async Task<IEnumerable<JobDto>> Handle(JobsQuery request,
                                                  CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des jobs...");

        var recurringJobs = await _jobManager.GetRecurringJobsAsync(cancellationToken)!.ToList();
        var jobsSettings = await _jobSettingStore.GetAllAsync(cancellationToken)!.ToDictionary(x => x.Identifiant!, true);

        var jobsDto = new List<JobDto>();

        foreach (var recurringJob in recurringJobs)
        {
            var jobDto = _mapper.Map<JobDto>(recurringJob);

            var jobSetting = jobsSettings!.GetValueOrDefault(recurringJob.Identifiant);
            if (jobSetting != null)
            {
                _mapper.MapIfExist(jobSetting, jobDto);
            }

            jobsDto.Add(jobDto);
        }

        _logger.LogInformation($"Récupération de {jobsDto.Count} jobs.");

        return jobsDto;
    }
}