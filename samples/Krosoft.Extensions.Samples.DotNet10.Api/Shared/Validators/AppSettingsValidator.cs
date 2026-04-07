using FluentValidation;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Shared.Validators;

internal class AppSettingsValidator : AbstractValidator<AppSettings>;