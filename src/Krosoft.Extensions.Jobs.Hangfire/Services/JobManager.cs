using AutoMapper;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Jobs.Hangfire.Services;

public class JobManager : IJobManager
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IEnumerable<IJob> _jobs;
    private readonly IJobsSettingStorageProvider _jobsSettingStorageProvider;
    private readonly JobStorage _jobStorage;
    private readonly ILogger<JobManager> _logger;
    private readonly IMapper _mapper;
    private readonly IMonitoringApi _monitoringApi;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IEnumerable<IRecurringJob> _recurringjobs;

    public JobManager(ILogger<JobManager> logger,
                      IRecurringJobManager recurringJobManager,
                      IEnumerable<IJob> jobs,
                      IEnumerable<IRecurringJob> recurringjobs,
                      JobStorage jobStorage,
                      IMapper mapper,
                      IJobsSettingStorageProvider jobsSettingStorageProvider,
                      IBackgroundJobClient backgroundJobClient)
    {
        _recurringJobManager = recurringJobManager;
        _logger = logger;
        _jobs = jobs;
        _recurringjobs = recurringjobs;
        _jobStorage = jobStorage;
        _mapper = mapper;
        _jobsSettingStorageProvider = jobsSettingStorageProvider;
        _backgroundJobClient = backgroundJobClient;
        _monitoringApi = JobStorage.Current.GetMonitoringApi();
    }

    public async Task AddJobAsync(JobContext jobContext, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des settings des jobs...");

        var job = _jobs.FirstOrDefault(x => x.Type == jobContext.TypeCode);
        if (job != null)
        {
            _logger.LogInformation($"Ajout du job {jobContext.Cle} en file...");
            _backgroundJobClient.Create(() => job.ExecuteAsync(jobContext, cancellationToken), new EnqueuedState(jobContext.QueueName));
        }
        else
        {
            throw new KrosoftTechnicalException($"Job introuvable pour le type {jobContext.TypeCode}");
        }

        await Task.CompletedTask;
    }

    public async Task AddOrUpdateRecurringJobsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des settings des jobs...");
        var jobsSetting = await _jobsSettingStorageProvider.GetAsync(cancellationToken);
        foreach (var jobSetting in jobsSetting)
        {
            var recurringJob = _recurringjobs.FirstOrDefault(x => x.Type == jobSetting.Type);
            if (recurringJob != null)
            {
#if NET9_0_OR_GREATER
                _recurringJobManager.AddOrUpdate(jobSetting.Identifiant,
                                                 jobSetting.QueueName,
                                                 () => recurringJob.ExecuteAsync(jobSetting.Identifiant!),
                                                 jobSetting.CronExpression,
                                                 new RecurringJobOptions
                                                 {
                                                     TimeZone = TimeZoneInfo.Local
                                                 });
#else
                _recurringJobManager.AddOrUpdate(jobSetting.Identifiant,
                                                 () => recurringJob.ExecuteAsync(jobSetting.Identifiant!),
                                                 jobSetting.CronExpression, queue: jobSetting.QueueName);
#endif

                _logger.LogInformation($"Ajout du recurring job '{jobSetting.Identifiant}' de type '{recurringJob.Type}' sur la queue '{jobSetting.QueueName}'.");
            }
            else
            {
                throw new KrosoftTechnicalException($"RecurringJob introuvable pour le type {jobSetting.Type}");
            }
        }
    }

    public async Task<IEnumerable<JobContext>> GetEnqueuedJobsAsync(string? queueName, CancellationToken cancellationToken)
    {
        var jobsData = _monitoringApi.EnqueuedJobs(queueName, 0, int.MaxValue)
                                     .Select(x => x.Value);

        var jobsContext = new List<JobContext>();
        foreach (var jobData in jobsData)
        {
            if (jobData.Job.Args != null && jobData.Job.Args.Any())
            {
                var jobContext = jobData.Job.Args[0] as JobContext;
                if (jobContext != null)
                {
                    jobsContext.Add(jobContext);
                }
            }
        }

        await Task.CompletedTask;

        return jobsContext;
    }

    public Task<IEnumerable<CronJob>> GetRecurringJobsAsync(CancellationToken cancellationToken)
    {
        List<RecurringJobDto>? recurringJobs = _jobStorage.GetConnection()
                                                          .GetRecurringJobs();

        return Task.FromResult(MapRecurringJobs(recurringJobs));
    }

    public Task<IEnumerable<CronJob>> GetRecurringJobsAsync(ISet<string> ids,
                                                            CancellationToken cancellationToken)
    {
        List<RecurringJobDto>? recurringJobs = _jobStorage.GetConnection()
                                                          .GetRecurringJobs(ids);

        return Task.FromResult(MapRecurringJobs(recurringJobs));
    }

    public async Task RemoveAsync(ISet<string> identifiants, CancellationToken cancellationToken)
    {
        foreach (var identifiant in identifiants)
        {
            await RemoveAsync(identifiant, cancellationToken);
        }
    }

    public Task RemoveAsync(string? identifiant, CancellationToken cancellationToken)
    {
        _recurringJobManager.RemoveIfExists(identifiant);
        return Task.CompletedTask;
    }

    public Task RemoveAllAsync(CancellationToken cancellationToken)
    {
        var recurringJobs = _jobStorage.GetConnection()
                                       .GetRecurringJobs();

        if (recurringJobs != null)
        {
            foreach (var recurringJob in recurringJobs)
            {
                _recurringJobManager.RemoveIfExists(recurringJob.Id);
            }
        }

        return Task.CompletedTask;
    }

    public Task TriggerAsync(CancellationToken cancellationToken)
    {
        var recurringJobs = _jobStorage.GetConnection().GetRecurringJobs();
        foreach (var recurringJob in recurringJobs)
        {
            _recurringJobManager.Trigger(recurringJob.Id);
        }

        return Task.CompletedTask;
    }

    public Task TriggerAsync(string? identifiant, CancellationToken cancellationToken)
    {
        _recurringJobManager.Trigger(identifiant);
        return Task.CompletedTask;
    }

    public Task<SystemStatistics> GetStatisticsAsync(CancellationToken cancellationToken)
    {
        var stats = _monitoringApi.GetStatistics();
        var servers = _monitoringApi.Servers();

        return Task.FromResult(new SystemStatistics
        {
            Enqueued = stats.Enqueued,
            Processing = stats.Processing,
            Succeeded = stats.Succeeded,
            Failed = stats.Failed,
            Servers = servers.Select(s => new SystemServer
            {
                Name = s.Name,
                WorkersCount = s.WorkersCount,
                Queues = s.Queues,
                StartedAt = s.StartedAt,
                Heartbeat = s.Heartbeat
            })
        });
    }

    private IEnumerable<CronJob> MapRecurringJobs(IEnumerable<RecurringJobDto> recurringJobs)
    {
        var jobs = new List<CronJob>();

        foreach (var recurringJob in recurringJobs)
        {
            var job = _mapper.Map<CronJob>(recurringJob);
            jobs.Add(job);
        }

        return jobs;
    }
}