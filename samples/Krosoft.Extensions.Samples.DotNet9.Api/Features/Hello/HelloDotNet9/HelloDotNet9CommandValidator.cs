using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.HelloDotNet9;

internal class HelloDotNet9CommandValidator : AbstractValidator<HelloDotNet9Command>
{
    public HelloDotNet9CommandValidator()
    { 
        RuleFor(c => c.Name).NotEmpty().NotNull();
    }
}