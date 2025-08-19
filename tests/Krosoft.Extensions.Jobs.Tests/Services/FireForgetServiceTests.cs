using Krosoft.Extensions.Jobs.Extensions;
using Krosoft.Extensions.Jobs.Interfaces;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Jobs.Services.Tests
{
    [TestClass]
    public class FireForgetServiceTests : BaseTest
    {
        protected override void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddFireForget();
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
            var services = new ServiceCollection();
            services.AddFireForget();
            var buildServiceProvider = services.BuildServiceProvider();
            var service = buildServiceProvider.GetRequiredService<IFireForgetService>();
            service.FireAsync<INotificationService>(null!);

            //Check.That(delta).IsLessThan(threshold);

            //Assert.Fail();
        }
    }
}

public class INotificationService
{
//    private readonly IFireForgetService _fireForgetService;
//    private readonly IPositiveTokenBuilderService _positiveTokenBuilderService;
//    public INotificationService(IFireForgetService fireForgetService, IPositiveTokenBuilderService positiveTokenBuilderService)
//    {
//        _fireForgetService = fireForgetService;
//        _positiveTokenBuilderService = positiveTokenBuilderService;
//    }
//    public void Publish(INotification notification, CancellationToken cancellationToken)
//    {
//        _fireForgetService.FireAsync<IMediator>(async mediator =>
//        {
//            var positiveToken = _positiveTokenBuilderService.Build();
//            await mediator.Publish(notification, cancellationToken);
//        });
//    }

//    public void Publish(Func<KrosoftToken, INotification> func, CancellationToken cancellationToken)
//{
//    _fireForgetService.FireAsync<IMediator>(async mediator =>
//    {
//        var positiveToken = _positiveTokenBuilderService.Build();

//        await mediator.Publish(func(positiveToken), cancellationToken);
//    });
}