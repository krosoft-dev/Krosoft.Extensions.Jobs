using AutoMapper;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Mapping.Extensions;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;

public class JobsQueryHandler : IRequestHandler<JobsQuery, IEnumerable<JobDto>>
{
    private readonly IJobManager _jobManager;
    private readonly IJobsSettingStorageProvider _jobsSettingStorageProvider;
    private readonly ILogger<JobsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public JobsQueryHandler(ILogger<JobsQueryHandler> logger,
                            IMapper mapper,
                            IJobManager jobManager,
                            IJobsSettingStorageProvider jobsSettingStorageProvider)
    {
        _logger = logger;
        _mapper = mapper;
        _jobManager = jobManager;
        _jobsSettingStorageProvider = jobsSettingStorageProvider;
    }

    public async Task<IEnumerable<JobDto>> Handle(JobsQuery request,
                                                  CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des jobs...");

        var jobsDto = new List<JobDto>();

        var recurringJobs = await _jobManager.GetRecurringJobsAsync(cancellationToken)!.ToDictionary(x => x.Identifiant!, true);

        var jobsSettings = await _jobsSettingStorageProvider.GetAsync(cancellationToken)!.ToList();
        if (jobsSettings.Any())
        {
            foreach (var jobAutomatique in jobsSettings)
            {
                var jobDto = _mapper.Map<JobDto>(jobAutomatique);

                var job = recurringJobs!.GetValueOrDefault(jobDto.Identifiant);
                _mapper.MapIfExist(job, jobDto);

                jobsDto.Add(jobDto);
            }
        }

        _logger.LogInformation($"Récupération de {jobsDto.Count} jobs.");

        return jobsDto;
    }
}