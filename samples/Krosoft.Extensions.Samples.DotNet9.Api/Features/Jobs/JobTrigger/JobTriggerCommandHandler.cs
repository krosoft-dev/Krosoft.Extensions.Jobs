using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.JobTrigger;

internal class JobTriggerCommandHandler : IRequestHandler<JobTriggerCommand>
{
    private readonly IJobManager _jobManager;
    private readonly IJobsSettingStorageProvider _jobsSettingStorageProvider;
    private readonly ILogger<JobTriggerCommandHandler> _logger;

    public JobTriggerCommandHandler(ILogger<JobTriggerCommandHandler> logger,
                                    IJobManager jobManager,
                                    IJobsSettingStorageProvider jobsSettingStorageProvider)
    {
        _logger = logger;
        _jobManager = jobManager;
        _jobsSettingStorageProvider = jobsSettingStorageProvider;
    }

    public async Task Handle(JobTriggerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Run du job {request.Identifiant}...");

        var jobsSettings = await _jobsSettingStorageProvider.GetAsync(cancellationToken);
        var jobAutomatiqueSetting = jobsSettings.FirstOrDefault(x => x.Identifiant == request.Identifiant);

        if (jobAutomatiqueSetting == null)
        {
            throw new JobIntrouvableException(request.Identifiant);
        }

        await _jobManager.TriggerAsync(request.Identifiant, cancellationToken);
    }
}