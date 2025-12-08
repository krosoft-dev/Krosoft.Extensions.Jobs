using Hangfire;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Jobs.Hangfire.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Ajoute Hangfire avec configuration étendue
    /// </summary>
    /// <param name="services">Collection de services</param>
    /// <param name="action">Configuration des options</param>
    /// <returns>Collection de services pour chaînage</returns>
    public static IServiceCollection AddHangfireExt(this IServiceCollection services,
                                                    Action<KrosoftBackgroundJobServerOptions> action)
    {
        var options = new KrosoftBackgroundJobServerOptions();
        action(options);

        // Validation du provider de stockage
        if (options.StorageProvider == null)
        {
            throw new KrosoftTechnicalException("Un provider de stockage doit être configuré.");
        }

        // Configuration Hangfire avec le provider de stockage
        services.AddHangfire(config => { options.StorageProvider.ConfigureStorage(config); });

        // Ajout du serveur Hangfire
        services.AddSingleton<BackgroundJobServerOptions>(options);
        services.AddHangfireServer();

        // Services métier
        services.AddScoped<IJobManager, JobManager>();
        services.AddHostedService<JobsStartupHostedService>();

        return services;
    }
}