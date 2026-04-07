using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Samples.Shared.Models;

public class JobIntrouvableException(string id) : KrosoftTechnicalException($"Job '{id}' introuvable.");