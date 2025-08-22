using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.HelloDotNet9;

internal record HelloDotNet9Command(string Name) : BaseCommand<string>;