using AutoMapper;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Services;

internal class SettingsJobsSettingStorageProvider : IJobsSettingStorageProvider
{
    private readonly IMapper _mapper;
    private readonly IOptions<AppSettings> _options;

    public SettingsJobsSettingStorageProvider(IMapper mapper, IOptions<AppSettings> options)
    {
        _mapper = mapper;
        _options = options;
    }

    public Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken)
    {
        var jobsSettings = new List<IJobAutomatiqueSetting>();
        foreach (var jobSettings in _options.Value.JobsAmqp)
        {
            jobsSettings.Add(_mapper.Map<JobAutomatiqueSetting>(jobSettings));
        }

        return Task.FromResult<IEnumerable<IJobAutomatiqueSetting>>(jobsSettings);
    }
}