using Hangfire;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Extensions;

/// <summary>
/// Conversion de <see cref="MissedExecutionPolicy" /> vers <see cref="MisfireHandlingMode" />.
/// </summary>
public static class MissedExecutionPolicyExtensions
{
    /// <summary>
    /// Convertit la politique de rattrapage en mode Hangfire. <c>null</c> donne <see cref="MisfireHandlingMode.Relaxed" />.
    /// </summary>
    /// <param name="policy">Politique de rattrapage.</param>
    /// <returns>Le mode Hangfire correspondant.</returns>
    /// <exception cref="KrosoftTechnicalException">Si la politique n'est pas gérée.</exception>
    public static MisfireHandlingMode ToMisfireHandlingMode(this MissedExecutionPolicy? policy)
        => policy switch
        {
            null or MissedExecutionPolicy.CatchUpOnce => MisfireHandlingMode.Relaxed,
            MissedExecutionPolicy.CatchUpAll => MisfireHandlingMode.Strict,
            MissedExecutionPolicy.Skip => MisfireHandlingMode.Ignorable,
            _ => throw new KrosoftTechnicalException($"Politique de rattrapage non gérée : {policy}.")
        };
}
