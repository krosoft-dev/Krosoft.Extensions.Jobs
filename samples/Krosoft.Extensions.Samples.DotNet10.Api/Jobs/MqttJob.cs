using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Jobs;

internal class MqttJob : IRecurringJob
{
    private readonly ILogger<MqttJob> _logger;

    public MqttJob(ILogger<MqttJob> logger)
    {
        _logger = logger;
    }

    public string Type => nameof(JobTypeCode.Mqtt);

    public async Task<JobResult> ExecuteAsync(string identifiant)
    {
        Guard.IsNotNull(nameof(identifiant), identifiant);

        var cancellationToken = CancellationToken.None;
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Exécution du job Mqtt '{identifiant}'...");

        try
        {
            _logger.LogInformation($"Exécution du job Mqtt '{identifiant}'...");
            await Task.Delay(2000, cancellationToken);

            return new JobResult(identifiant, sw.Elapsed, null);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exécution du job Mqtt '{identifiant}' en erreur : {e.Message}.", e);
            throw;
        }
        finally
        {
            _logger.LogInformation($"Exécution du job Mqtt '{identifiant}' terminée en {sw.Elapsed.ToShortString()}.");
        }
    }
}