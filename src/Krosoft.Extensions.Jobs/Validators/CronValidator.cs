using Cronos;
using FluentValidation;
using FluentValidation.Validators;

namespace Krosoft.Extensions.Jobs.Validators;

public class CronValidator<T> : PropertyValidator<T, string?>
{
    public override string Name => nameof(CronValidator<T>);

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var isValid = CronExpression.TryParse(value, out _);

        return isValid;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "L'expression CRON est invalide.";
}