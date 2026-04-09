using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.HelloDotNet9;

internal class HelloDotNet10CommandHandler : IRequestHandler<HelloDotNet10Command, string>
{
    private readonly ILogger<HelloDotNet10CommandHandler> _logger;

    public HelloDotNet10CommandHandler(ILogger<HelloDotNet10CommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloDotNet10Command request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Hello {request.Name}...");

        return Task.FromResult($"Hello {request.Name} !");
    }
}