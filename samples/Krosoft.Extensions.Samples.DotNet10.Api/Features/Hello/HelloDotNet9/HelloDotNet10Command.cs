using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.HelloDotNet9;

internal record HelloDotNet10Command(string Name) : BaseCommand<string>;