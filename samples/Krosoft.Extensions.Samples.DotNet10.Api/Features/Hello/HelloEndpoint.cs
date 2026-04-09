using Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.HelloDotNet9;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello;

internal class HelloEndpoint : IEndpoint
{
    public void Register(RouteGroupBuilder group)
    {
        group.MapPost("/", (HelloDotNet10CommandDto dto,
                            IMediator mediator,
                            CancellationToken cancellationToken)
                          => mediator.Send(new HelloDotNet10Command(dto.Name), cancellationToken));
    }

    public RouteGroupBuilder DefineGroup(WebApplication app) => app.MapGroup("/Hello")
                                                                   .DisableAntiforgery()
                                                                   .WithTags("Hello");
}