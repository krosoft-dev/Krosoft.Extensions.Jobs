using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.Shared.Models;

namespace Krosoft.Extensions.Samples.Worker1.Jobs;

internal class Worker1Job : IRecurringJob
{
    private readonly ILogger<Worker1Job> _logger;

    public Worker1Job(ILogger<Worker1Job> logger)
    {
        _logger = logger;
    }

    public string Type => nameof(JobTypeCode.Worker1);

    public async Task<JobResult> ExecuteAsync(string identifiant)
    {
        Guard.IsNotNull(nameof(identifiant), identifiant);

        var cancellationToken = CancellationToken.None;
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Exécution du job Worker1 '{identifiant}'...");

        try
        {
            _logger.LogInformation($"Exécution du job Worker1 '{identifiant}'...");
            await Task.Delay(2000, cancellationToken);

            return new JobResult(identifiant, sw.Elapsed, null);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exécution du job Worker1 '{identifiant}' en erreur : {e.Message}.", e);
            throw;
        }
        finally
        {
            _logger.LogInformation($"Exécution du job Worker1 '{identifiant}' terminée en {sw.Elapsed.ToShortString()}.");
        }
    }
}