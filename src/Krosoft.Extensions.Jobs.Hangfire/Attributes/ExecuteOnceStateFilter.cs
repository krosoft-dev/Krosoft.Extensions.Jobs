using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;

namespace Krosoft.Extensions.Jobs.Hangfire.Attributes;

/// <summary>
/// Filtre global qui nettoie le hash fingerprint quand un job est supprimé via le dashboard.
/// Complète <see cref="ExecuteOnceAttribute"/> pour le cas où le filtre d'attribut n'est pas appelé
/// (ex: DynamicJob dont le type réel porte l'attribut, mais pas le type Hangfire enregistré).
/// Ne fait rien pour les jobs sans fingerprint (RemoveHash est un no-op si la clé n'existe pas).
/// </summary>
public class ExecuteOnceStateFilter : JobFilterAttribute, IApplyStateFilter
{
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is not DeletedState || context.BackgroundJob.Job == null)
        {
            return;
        }

        var fingerprintKey = context.BackgroundJob.Job.GetFingerprintKey();
        var entries = context.Connection.GetAllEntriesFromHash(fingerprintKey);
        if (entries is { Count: > 0 })
        {
            transaction.RemoveHash(fingerprintKey);
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }
}
