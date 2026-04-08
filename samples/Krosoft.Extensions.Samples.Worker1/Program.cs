using System.Reflection;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Profiles;
using Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Extensions;
using Krosoft.Extensions.Samples.Shared.Jobs;
using Krosoft.Extensions.Samples.Shared.Models;
using Krosoft.Extensions.Samples.Worker1.Jobs;
using Krosoft.Extensions.Samples.Worker1.Services;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Extensions;

var currentAssembly = Assembly.GetExecutingAssembly();

var assemblies = new[]
{
    currentAssembly,
    typeof(HangfireProfile).Assembly
};

var builder = WebApplication.CreateBuilder(args);
builder.Services
       .AddWebApi(builder.Configuration, assemblies)
       .AddHangfireExt(options =>
       {
           options.Queues =
           [
               Constants.QueuesKeys.System,
               Constants.QueuesKeys.Worker1
           ];
           options.WorkerCount = 1;
           options.UseRedisStorage("krosoft.redis:6379,password=EI0hKU2OBfPHNPfd");
       })
       .AddTransient<IJobsSettingStorageProvider, WorkerJobsSettingStorageProvider>().AddHostedService<ExecuteOnceCleanupService>()
       .AddTransient<IRecurringJob, Worker1Job>()
       .AddTransient<IRecurringJob, SharedJob>()
       .AddTransient<IRecurringJob, SoLongJob>()
       .AddHealthChecks();

var app = builder.Build();
app.UseWebApi(builder.Environment, builder.Configuration,
              x => x.UseHealthChecksExt(builder.Environment),
              endpoints => endpoints.MapHealthChecksExt());

await app.RunAsync();