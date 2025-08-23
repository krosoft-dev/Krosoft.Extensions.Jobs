using FluentValidation;
using Krosoft.Extensions.Jobs.Extensions;

namespace Krosoft.Extensions.Jobs.Tests.Core;

internal class TriggerCreateCommandValidator : AbstractValidator<TriggerCreateCommand>
{
    public TriggerCreateCommandValidator()
    {
        RuleFor(v => v.FlowId)
            .NotEmpty();
  
        RuleFor(v => v.CronExpression)
            .NotEmpty()
            .ValidCron();
    }
}