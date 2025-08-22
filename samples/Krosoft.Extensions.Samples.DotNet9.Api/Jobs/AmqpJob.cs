using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Jobs;

internal class AmqpJob : IRecurringJob
{
    private readonly ILogger<AmqpJob> _logger;
    private readonly IOptions<AppSettings> _options;

    public AmqpJob(ILogger<AmqpJob> logger, IOptions<AppSettings> options)
    {
        _logger = logger;
        _options = options;
    }

    public string Type => nameof(JobTypeCode.Amqp);

    public async Task<JobResult> ExecuteAsync(string identifiant)
    {
        Guard.IsNotNull(nameof(identifiant), identifiant);

        var cancellationToken = CancellationToken.None;
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Exécution du job '{identifiant}'...");

        try
        {
            var jobSetting = _options.Value.JobsAmqp.FirstOrDefault(x => x.Identifiant == identifiant);
            if (jobSetting == null)
            {
                throw new JobIntrouvableException(identifiant);
            }

            _logger.LogInformation($"Exécution du job '{identifiant}'...");
            await Task.Delay(2000, cancellationToken);

            return new JobResult("OK", sw.Elapsed, null);
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
}