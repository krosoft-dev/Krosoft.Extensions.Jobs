using FluentValidation;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Validators;

internal class JobAmqpSettingsValidator : AbstractValidator<JobAmqpSettings>
{
    public JobAmqpSettingsValidator()
    {
        RuleFor(v => v.CronExpression)
            .NotEmpty();
        RuleFor(v => v.Identifiant)
            .NotEmpty();
        RuleForEach(v => v.QueueName)
            .NotEmpty();
    }
}