using Krosoft.Extensions.Jobs.Extensions;
using Krosoft.Extensions.Jobs.Interfaces;
using Krosoft.Extensions.Jobs.Services;
using Krosoft.Extensions.Jobs.Tests.Core;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Jobs.Tests.Services;

[TestClass]
public class FireForgetServiceTests : BaseTest
{
    private Mock<ILogger<FireForgetService>> _mockLogger = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        _mockLogger = new Mock<ILogger<FireForgetService>>();
        services.SwapTransient(_ => _mockLogger.Object)
                .AddFireForget();
    }

    [TestMethod]
    public void FireTest()
    {
        using var serviceProvider = CreateServiceCollection();

        var service = serviceProvider.GetRequiredService<IFireForgetService>();

        service.Fire<INotificationService>(null!);

        //Check.That(service.Now.DateTime).IsEqualTo(new DateTime(2012, 1, 3));

        //Assert.Fail();
    }

    [TestMethod]
    public void FireAsyncTest()
    {
        using var serviceProvider = CreateServiceCollection();

        var service = serviceProvider.GetRequiredService<IFireForgetService>();
        service.FireAsync<INotificationService>(null!);

        //Check.That(delta).IsLessThan(threshold);

        //Assert.Fail();
    }
}