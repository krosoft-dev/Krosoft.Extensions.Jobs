using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;
using System.Net;
using JetBrains.Annotations;

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
}