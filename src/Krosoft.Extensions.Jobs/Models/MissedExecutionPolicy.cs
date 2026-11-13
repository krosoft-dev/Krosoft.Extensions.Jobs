namespace Krosoft.Extensions.Jobs.Models;

/// <summary>
/// Politique de rattrapage des exécutions manquées d'un job récurrent.
/// </summary>
public enum MissedExecutionPolicy
{
    /// <summary>
    /// Une seule exécution de rattrapage.
    /// </summary>
    CatchUpOnce,

    /// <summary>
    /// Toutes les exécutions manquées.
    /// </summary>
    CatchUpAll,

    /// <summary>
    /// Aucune exécution de rattrapage.
    /// </summary>
    Skip
}
