using FluentValidation;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Validators;

internal class AppSettingsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsValidator(IValidator<JobAmqpSettings> validator)
    {
        RuleForEach(v => v.JobsAmqp)
            .SetValidator(validator);
    }
}