using FluentValidation.TestHelper;
using Krosoft.Extensions.Jobs.Tests.Core;

namespace Krosoft.Extensions.Jobs.Tests.Validators;

[TestClass]
public class TriggerCreateCommandValidatorTests
{
    private TriggerCreateCommandValidator _validator = null!;

    [TestInitialize]
    public void Setup()
    {
        _validator = new TriggerCreateCommandValidator();
    }

    [TestMethod]
    public void Should_Have_Error_When_FlowId_Is_Empty()
    {
        var model = new TriggerCreateCommand { FlowId = string.Empty, CronExpression = "0 0 * * *" };

        var result = _validator.TestValidate(model);

        Check.That(result.IsValid).IsFalse();
        Check.That(result.Errors.Select(x => x.ErrorMessage)).ContainsExactly("'Flow Id' ne doit pas être vide.");
    }

    [TestMethod]
    public void Should_Have_Error_When_CronExpression_Is_Empty()
    {
        var model = new TriggerCreateCommand { FlowId = "valid-id", CronExpression = string.Empty };

        var result = _validator.TestValidate(model);

        Check.That(result.IsValid).IsFalse();
        Check.That(result.Errors.Select(x => x.ErrorMessage))
             .ContainsExactly("'Cron Expression' ne doit pas être vide.",
                              "L'expression CRON est invalide.");
    }

    [TestMethod]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new TriggerCreateCommand { FlowId = "valid-id", CronExpression = "0 0 * * *" };

        var result = _validator.TestValidate(model);

        Check.That(result.IsValid).IsTrue();
    }
}