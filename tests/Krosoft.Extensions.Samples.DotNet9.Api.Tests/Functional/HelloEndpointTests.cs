using System.Net;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Functional;

[TestClass]
[TestSubject(typeof(HelloEndpoint))]
public class HelloEndpointTests : SampleBaseApiTest<Program>
{
    [TestMethod]
    public async Task HelloName_Ok()
    {
        var url = "/Hello";
        var obj = new { Name = "World of Tests" };

        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsNewtonsoftJsonAsync(url, obj);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync(CancellationToken.None);
        Check.That(result).IsEqualTo("Hello World of Tests !");
    }

    [TestMethod]
    public async Task HelloName_Null()
    {
        var url = "/Hello";
        var obj = new { Name = (string?)null };

        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsNewtonsoftJsonAsync(url, obj);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsNewtonsoftJsonAsync<ErrorDto>();
        Check.That(error).IsNotNull();
        Check.That(error!.Code).IsEqualTo(400);
        Check.That(error.Errors)
             .ContainsExactly("'Name' ne doit pas être vide.",
                              "'Name' ne doit pas avoir la valeur null.");
    }
}