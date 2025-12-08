using Hangfire;
using Hangfire.InMemory;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Jobs.Hangfire.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHangfireExt(this IServiceCollection services,
                                                    Action<BackgroundJobServerOptions> action)
    {
        //services.AddHangfire(config => config.UseStorage(new InMemoryStorage()));
        services.AddHangfire(config => {});
        services.AddHangfireServer(action);
        services.AddScoped<IJobManager, JobManager>();
        services.AddHostedService<JobsStartupHostedService>();

        return services;
    }
}