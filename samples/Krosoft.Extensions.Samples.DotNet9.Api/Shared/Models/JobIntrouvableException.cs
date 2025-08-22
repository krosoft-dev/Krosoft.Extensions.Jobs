using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

public class JobIntrouvableException(string id) : KrosoftTechnicalException($"Job '{id}' introuvable.");