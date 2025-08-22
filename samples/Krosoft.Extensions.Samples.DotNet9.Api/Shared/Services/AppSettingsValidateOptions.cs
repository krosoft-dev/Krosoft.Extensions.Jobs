using FluentValidation;
using Krosoft.Extensions.Options.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Services;

internal class AppSettingsValidateOptions(IValidator<AppSettings> validator) : SettingsValidateOptions<AppSettings>(validator);

 