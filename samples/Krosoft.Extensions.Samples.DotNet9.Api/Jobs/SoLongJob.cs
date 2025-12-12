using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Attributes;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Jobs;

[ExecuteOnce]
internal class SoLongJob : IRecurringJob
{
    private readonly ILogger<SoLongJob> _logger;

    public SoLongJob(ILogger<SoLongJob> logger)
    {
        _logger = logger;
    }

    public string Type => nameof(JobTypeCode.SoLong);

    public async Task<JobResult> ExecuteAsync(string identifiant)
    {
        Guard.IsNotNull(nameof(identifiant), identifiant);

        var cancellationToken = CancellationToken.None;
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Exécution du job '{identifiant}'...");

        try
        {
            _logger.LogInformation($"Exécution du job '{identifiant}'...");
            await ProcessLongRunningTask(cancellationToken);

            return new JobResult(identifiant, sw.Elapsed, null);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exécution du job '{identifiant}' en erreur : {e.Message}.", e);
            throw;
        }
        finally
        {
            _logger.LogInformation($"Exécution du job '{identifiant}' terminée en {sw.Elapsed.ToShortString()}.");
        }
    }

    private static async Task ProcessLongRunningTask(CancellationToken cancellationToken)
    {
        // Simulation d'un traitement long
        await Task.Delay(TimeSpan.FromMinutes(7), cancellationToken);
    }
}