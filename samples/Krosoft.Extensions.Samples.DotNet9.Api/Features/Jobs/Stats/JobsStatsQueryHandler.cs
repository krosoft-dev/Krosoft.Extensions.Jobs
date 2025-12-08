using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Stats;

public class JobsStatsQueryHandler : IRequestHandler<JobsStatsQuery, SystemStatistics>
{
    private readonly IJobManager _jobManager;
    private readonly ILogger<JobsStatsQueryHandler> _logger;

    public JobsStatsQueryHandler(ILogger<JobsStatsQueryHandler> logger, IJobManager jobManager)
    {
        _logger = logger;
        _jobManager = jobManager;
    }

    public async Task<SystemStatistics> Handle(JobsStatsQuery request,
                                               CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des stats...");

        var stats = await _jobManager.GetStatisticsAsync(cancellationToken);

        return stats;
    }
}