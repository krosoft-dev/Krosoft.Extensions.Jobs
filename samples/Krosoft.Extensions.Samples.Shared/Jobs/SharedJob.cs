using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Samples.Shared.Jobs;

public class SharedJob : IRecurringJob
{
    private readonly ILogger<SharedJob> _logger;

    public SharedJob(ILogger<SharedJob> logger)
    {
        _logger = logger;
    }

    public string Type => nameof(JobTypeCode.Shared);

    public async Task<JobResult> ExecuteAsync(string identifiant)
    {
        Guard.IsNotNull(nameof(identifiant), identifiant);

        var cancellationToken = CancellationToken.None;
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Exécution du job Shared '{identifiant}'...");

        try
        {
            _logger.LogInformation($"Exécution du job Shared '{identifiant}'...");
            await Task.Delay(2000, cancellationToken);

            return new JobResult(identifiant, sw.Elapsed, null);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exécution du job Shared '{identifiant}' en erreur : {e.Message}.", e);
            throw;
        }
        finally
        {
            _logger.LogInformation($"Exécution du job Shared '{identifiant}' terminée en {sw.Elapsed.ToShortString()}.");
        }
    }
}