using Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.HelloDotNet9;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;
using Krosoft.Extensions.Testing.Cqrs.Extensions;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Unit.Features.Hello.HelloDotNet9;

[TestClass]
public class HelloDotNet9CommandHandlerTests : SampleBaseTest<Program>
{
    [TestMethod]
    public async Task Handle_Ok()
    {
        var serviceProvider = CreateServiceCollection();

        var result = await this.SendCommandAsync(serviceProvider, new HelloDotNet9Command("Luke"));
        Check.That(result).IsNotNull();
        Check.That(result).IsEqualTo("Hello Luke !");
    }
}