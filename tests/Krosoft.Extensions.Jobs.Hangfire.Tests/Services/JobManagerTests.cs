using AutoMapper;
using Hangfire;
using Hangfire.Storage;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Services;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Jobs.Hangfire.Tests.Services;

[TestClass]
public class JobManagerTests
{
    private Mock<IJobSettingStore> _jobSettingStore = null!;
    private Mock<IJobsSettingStorageProvider> _jobsSettingStorageProvider = null!;
    private Mock<IRecurringJobManager> _recurringJobManager = null!;

    [TestInitialize]
    public void Initialize()
    {
        _recurringJobManager = new Mock<IRecurringJobManager>();
        _jobsSettingStorageProvider = new Mock<IJobsSettingStorageProvider>();
        _jobSettingStore = new Mock<IJobSettingStore>();

        var jobStorage = new Mock<JobStorage>();
        jobStorage.Setup(x => x.GetMonitoringApi()).Returns(new Mock<IMonitoringApi>().Object);
        JobStorage.Current = jobStorage.Object;
    }

    [TestMethod]
    public async Task AddOrUpdateRecurringJobsAsync_SettingPlusFourni_SupprimeLeRecurringJobOrphelin()
    {
        var jobManager = CreateJobManager(["FlowTrigger"],
                                          [CreateSetting("actif", "FlowTrigger")],
                                          [CreateStoredSetting("actif", "FlowTrigger"), CreateStoredSetting("orphelin", "FlowTrigger")]);

        await jobManager.AddOrUpdateRecurringJobsAsync(CancellationToken.None);

        _recurringJobManager.Verify(x => x.RemoveIfExists("orphelin"), Times.Once);
        _jobSettingStore.Verify(x => x.RemoveAsync("orphelin", It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task AddOrUpdateRecurringJobsAsync_SettingFourni_NeSupprimePasLeRecurringJob()
    {
        var jobManager = CreateJobManager(["FlowTrigger"],
                                          [CreateSetting("actif", "FlowTrigger")],
                                          [CreateStoredSetting("actif", "FlowTrigger")]);

        await jobManager.AddOrUpdateRecurringJobsAsync(CancellationToken.None);

        _recurringJobManager.Verify(x => x.RemoveIfExists(It.IsAny<string>()), Times.Never);
        _jobSettingStore.Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task AddOrUpdateRecurringJobsAsync_TypeGereParUneAutreApplication_NeSupprimePasLeRecurringJob()
    {
        var jobManager = CreateJobManager(["FlowTrigger"],
                                          [CreateSetting("actif", "FlowTrigger")],
                                          [CreateStoredSetting("actif", "FlowTrigger"), CreateStoredSetting("autreApp", "PendingMessagesReclaim")]);

        await jobManager.AddOrUpdateRecurringJobsAsync(CancellationToken.None);

        _recurringJobManager.Verify(x => x.RemoveIfExists(It.IsAny<string>()), Times.Never);
        _jobSettingStore.Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private JobManager CreateJobManager(IEnumerable<string> typesGeres,
                                        IEnumerable<IJobAutomatiqueSetting> jobsSetting,
                                        IEnumerable<StoredJobSetting> storedSettings)
    {
        var recurringJobs = typesGeres.Select(type =>
        {
            var recurringJob = new Mock<IRecurringJob>();
            recurringJob.SetupGet(x => x.Type).Returns(type);
            return recurringJob.Object;
        }).ToList();

        _jobsSettingStorageProvider.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(jobsSetting);

        _jobSettingStore.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(storedSettings);

        return new JobManager(new Mock<ILogger<JobManager>>().Object,
                              _recurringJobManager.Object,
                              [],
                              recurringJobs,
                              JobStorage.Current,
                              new Mock<IMapper>().Object,
                              _jobsSettingStorageProvider.Object,
                              _jobSettingStore.Object,
                              new Mock<IBackgroundJobClient>().Object);
    }

    private static IJobAutomatiqueSetting CreateSetting(string identifiant, string type)
        => new JobAutomatiqueSetting
        {
            Identifiant = identifiant,
            CronExpression = "* * * * *",
            Type = type,
            QueueName = "default"
        };

    private static StoredJobSetting CreateStoredSetting(string identifiant, string type)
        => new()
        {
            Identifiant = identifiant,
            CronExpression = "* * * * *",
            Type = type,
            QueueName = "default"
        };
}
