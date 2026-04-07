using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.HelloDotNet9;

internal class HelloDotNet10CommandValidator : AbstractValidator<HelloDotNet10Command>
{
    public HelloDotNet10CommandValidator()
    { 
        RuleFor(c => c.Name).NotEmpty().NotNull();
    }
}