namespace Krosoft.Extensions.Jobs.Tests.Core;

internal interface INotificationService
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