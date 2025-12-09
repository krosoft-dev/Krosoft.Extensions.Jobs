using System.Reflection;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Validations.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Profiles;
using Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Storage.Redis.Extensions;
using Krosoft.Extensions.Options.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Jobs;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Services;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Extensions;
using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Krosoft.Extensions.WebApi.Swagger.HealthChecks.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var currentAssembly = Assembly.GetExecutingAssembly();

var assemblies = new[]
{
    currentAssembly,
    typeof(HangfireProfile).Assembly
};

var builder = WebApplication.CreateBuilder(args);

//Web API.
builder.Services
       .AddOptionsValidator<AppSettings, AppSettingsValidateOptions>(builder.Configuration)
       .AddWebApi(builder.Configuration, assemblies)
       //CQRS.
       .AddBehaviors(options => options.AddLogging()
                                       .AddValidations())
       //Swagger.
       .AddSwagger(currentAssembly, options => options.AddHealthChecks()
                                                      .AddGlobalResponses())

//Jobs
       .AddHangfireExt(options =>
       {
           options.Queues =
           [
               Constantes.QueuesKeys.Default,
               Constantes.QueuesKeys.Prio,
               Constantes.QueuesKeys.System,
           ];
           options.WorkerCount = 21;
           //options.UseInMemoryStorage();
           options.UseRedisStorage("krosoft.redis:6379,password=EI0hKU2OBfPHNPfd");
       })
       .AddTransient<IJobsSettingStorageProvider, SettingsJobsSettingStorageProvider>()
       .AddTransient<IRecurringJob, AmqpJob>()
       .AddTransient<IRecurringJob, SoLongJob>()

//Autres
       .AddDateTimeService()
       .AddHealthChecks()
       .AddCheck("Test_Endpoint", () => HealthCheckResult.Healthy())
    ;

var app = builder.Build();
app.UseWebApi(builder.Environment, builder.Configuration,
              x => x
                  .UseHealthChecksExt(builder.Environment),
              endpoints => endpoints.MapHealthChecksExt())
   .UseSwaggerExt()
   .UseHangfire(builder.Configuration);

await app
      .AddEndpoints(currentAssembly)
      .RunAsync();

namespace Krosoft.Extensions.Samples.DotNet9.Api
{
    public class Program;
}