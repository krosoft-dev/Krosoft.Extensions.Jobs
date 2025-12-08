using System.Net;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Functional;

[TestClass]
[TestSubject(typeof(JobsEndpoint))]
public class JobsEndpointTests : SampleBaseApiTest<Program>
{
    [TestMethod]
    public async Task Get_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Jobs");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var jobs = await response.Content.ReadAsNewtonsoftJsonAsync<IEnumerable<JobDto>>(CancellationToken.None).ToList();
        Check.That(jobs).IsNotNull();
        Check.That(jobs).HasSize(1);
        Check.That(jobs.Select(x => x.Identifiant)).ContainsExactly("CHECK");
    }

    [TestMethod]
    public async Task Get_Stats_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Jobs/Stats");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var statistics = await response.Content.ReadAsNewtonsoftJsonAsync<SystemStatistics>(CancellationToken.None);
        Check.That(statistics).IsNotNull(); 
        Check.That(statistics!.Servers).IsNotNull(); 
        Check.That(statistics.Servers.SelectMany(x => x.Queues).ToHashSet()).ContainsExactly("default", "prio");
    }

    [TestMethod]
    public async Task Run_Ko()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Jobs/Run/Test", null);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.InternalServerError);

        var errorDto = await response.Content.ReadAsNewtonsoftJsonAsync<ErrorDto>(CancellationToken.None);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Code).IsEqualTo(500);
        Check.That(errorDto.Errors).HasSize(1);
        Check.That(errorDto.Errors).ContainsExactly("Job 'Test' introuvable.");
    }

    [TestMethod]
    public async Task Run_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Jobs/Run/CHECK", null);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        Check.That(content).IsEmpty();
    }
}