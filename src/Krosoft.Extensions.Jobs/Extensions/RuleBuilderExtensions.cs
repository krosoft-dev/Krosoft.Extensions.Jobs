using FluentValidation;
using Krosoft.Extensions.Jobs.Validators;

namespace Krosoft.Extensions.Jobs.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string?> ValidCron<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        var validator = new CronValidator<T>();

        return ruleBuilder.SetValidator(validator);
    }
}