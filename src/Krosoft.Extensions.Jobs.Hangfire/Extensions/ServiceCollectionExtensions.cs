using System.Reflection;
using Hangfire;
using Hangfire.Common;
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

        var appName = Assembly.GetEntryAssembly()?.GetName().Name;
        var machineName = Environment.MachineName;

        options.ServerName = $"{appName}_{machineName}";

        // Validation du provider de stockage
        if (options.StorageProvider == null)
        {
            throw new KrosoftTechnicalException("Un provider de stockage doit être configuré.");
        }

        // Configuration Hangfire avec le provider de stockage
        services.AddHangfire(config =>
        {
            options.StorageProvider.ConfigureStorage(config);
#if NET9_0_OR_GREATER
            config.UseDynamicJobs();
#endif
        });

        // Ajout du serveur Hangfire
        services.AddSingleton<BackgroundJobServerOptions>(options);
        services.AddHangfireServer();

        // Services métier
        services.AddScoped<IJobManager, JobManager>();
        services.AddHostedService<JobsStartupHostedService>();

        return services;
    }
}

public static class JobExtensions
{
    //public static bool SkipConcurrentExecution(this Job job)
    //    => job.Method.GetCustomAttributes(typeof(SkipConcurrentExecutionAttribute), false).Length > 0;

    public static string GetFingerprintLockKey(this Job job) => $"{job.GetFingerprintKey()}:lock";
    public static string GetFingerprintKey(this Job job) => $"fingerprint:{job.GetFingerprint()}";

    private static string GetFingerprint(this Job job)
    {
        //if (job.Type == null || job.Method == null) { return string.Empty; }
        //var parameters = string.Empty;

        //if (job.Args is not null) { parameters = string.Join(".", job.Args); }

        //return $"{job.Type.FullName}.{job.Method.Name}.{parameters}";

        if (job.Type == null || job.Method == null)
        {
            return string.Empty;
        }

        string typeName;
        string methodName;
        string parameters;


#if NET9_0_OR_GREATER

        if (job.Type == typeof(DynamicJob) && job.Args is { Count: > 0 } && job.Args[0] is DynamicJob dynamicJob)
        {
            typeName = dynamicJob.Type ?? string.Empty;
            methodName = dynamicJob.Method ?? string.Empty;
            parameters = dynamicJob.Args ?? string.Empty;
        }
        else
        {
            typeName = job.Type.FullName ?? string.Empty;
            methodName = job.Method.Name;
            parameters = job.Args is not null ? string.Join(".", job.Args) : string.Empty;
        }
        #else
           typeName = job.Type.FullName ?? string.Empty;
            methodName = job.Method.Name;
            parameters = job.Args is not null ? string.Join(".", job.Args) : string.Empty;
#endif


       

        return $"{typeName}.{methodName}.{parameters}";
    }
}