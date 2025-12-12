using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;
using Krosoft.Extensions.Testing.WebApi;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

//public abstract class SampleBaseApiTest<TEntry> : BaseApiTest<TEntry, KrosoftExtensionTenantContext> where TEntry : class
public abstract class SampleBaseApiTest<TEntry> : BaseApiTest<TEntry, TEntry> where TEntry : class
{
    protected override void ConfigureClaims(KrosoftToken krosoftToken)
    {
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        //// Remove DbContext registration.
        //services.RemoveService(d => d.ServiceType == typeof(DbContextOptions<KrosoftExtensionTenantContext>));
        //services.AddDbContextInMemory<KrosoftExtensionTenantContext>(true);
        //services.AddSeedService<KrosoftExtensionTenantContext, SampleSeedService>();

        //services.MockRedis();
        //services.AddTransient<IDistributedCacheProvider, DictionaryCacheProvider>();

        //// Remove IHostedService registration.
        //services.RemoveService(d => d.ImplementationType == typeof(SampleBackgroundService));

        //// Mock Pub-Sub services.
        //var mock = new Mock<IPublisherService>();
        //mock.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
        //    .Returns(() => Task.CompletedTask);
        //services.SwapTransient(_ => mock.Object);

        services.AddHangfireExt(options =>
        {
            options.Queues =
            [
                Constantes.QueuesKeys.Default,
                Constantes.QueuesKeys.Prio
            ];
            options.WorkerCount = 1;
            // Force InMemory pour les tests.
            options.UseInMemoryStorage();
        });
    }
}