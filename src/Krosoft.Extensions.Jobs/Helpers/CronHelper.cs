using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Cronos;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Jobs.Helpers;

/// <summary>
/// Utilitaires pour répartir l'exécution des expressions CRON dans la minute.
/// </summary>
public static class CronHelper
{
    /// <summary>
    /// Calcule une seconde de décalage (0 à 59) déterministe à partir d'une clé.
    /// </summary>
    /// <param name="key">Clé servant au calcul du décalage.</param>
    /// <returns>Une seconde comprise entre 0 et 59.</returns>
    /// <exception cref="KrosoftTechnicalException">Si la clé est null ou vide.</exception>
    public static int GetSecondOffset(string key)
    {
        Guard.IsNotNullOrEmpty(nameof(key), key);

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var value = BinaryPrimitives.ReadUInt32LittleEndian(hash);

        return (int)(value % 60);
    }

    /// <summary>
    /// Préfixe une expression CRON à 5 champs par une seconde déterminée à partir de la clé.
    /// Toute autre expression est renvoyée telle quelle.
    /// </summary>
    /// <param name="cronExpression">Expression CRON à répartir.</param>
    /// <param name="key">Clé servant au calcul du décalage.</param>
    /// <returns>L'expression CRON à 6 champs, ou l'expression d'origine.</returns>
    public static string? SpreadWithinMinute(string? cronExpression, string key)
    {
        if (string.IsNullOrWhiteSpace(cronExpression))
        {
            return cronExpression;
        }

        var fields = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (fields.Length != 5)
        {
            return cronExpression;
        }

        return $"{GetSecondOffset(key)} {string.Join(' ', fields)}";
    }

    /// <summary>
    /// Calcule le plus grand écart entre deux occurrences consécutives d'une expression CRON, à partir de maintenant.
    /// </summary>
    /// <param name="cronExpression">Expression CRON à 5 ou 6 champs.</param>
    /// <param name="timeZone">Fuseau horaire d'évaluation de l'expression.</param>
    /// <returns>Le plus grand écart, ou null si l'expression est vide, invalide ou a moins de deux occurrences.</returns>
    public static TimeSpan? GetMaxInterval(string? cronExpression, TimeZoneInfo timeZone)
        => GetMaxInterval(cronExpression, DateTime.UtcNow, timeZone);

    /// <summary>
    /// Calcule le plus grand écart entre deux occurrences consécutives d'une expression CRON, sur environ un jour à partir de <paramref name="fromUtc" />.
    /// </summary>
    /// <param name="cronExpression">Expression CRON à 5 ou 6 champs.</param>
    /// <param name="fromUtc">Date UTC de début d'énumération des occurrences.</param>
    /// <param name="timeZone">Fuseau horaire d'évaluation de l'expression.</param>
    /// <returns>Le plus grand écart, ou null si l'expression est vide, invalide ou a moins de deux occurrences.</returns>
    public static TimeSpan? GetMaxInterval(string? cronExpression, DateTime fromUtc, TimeZoneInfo timeZone)
    {
        var expression = TryParse(cronExpression);
        if (expression == null)
        {
            return null;
        }

        var from = fromUtc.Kind == DateTimeKind.Local
            ? fromUtc.ToUniversalTime()
            : DateTime.SpecifyKind(fromUtc, DateTimeKind.Utc);
        var limit = from.AddDays(1);

        var previous = expression.GetNextOccurrence(from, timeZone);
        TimeSpan? maxInterval = null;
        while (previous.HasValue)
        {
            var next = expression.GetNextOccurrence(previous.Value, timeZone);
            if (!next.HasValue)
            {
                break;
            }

            var interval = next.Value - previous.Value;
            if (maxInterval == null || interval > maxInterval)
            {
                maxInterval = interval;
            }

            if (previous.Value > limit)
            {
                break;
            }

            previous = next;
        }

        return maxInterval;
    }

    private static CronExpression? TryParse(string? cronExpression)
    {
        if (string.IsNullOrWhiteSpace(cronExpression))
        {
            return null;
        }

        var fields = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var format = fields.Length switch
        {
            5 => CronFormat.Standard,
            6 => CronFormat.IncludeSeconds,
            _ => (CronFormat?)null
        };

        if (format == null)
        {
            return null;
        }

        try
        {
            return CronExpression.Parse(string.Join(' ', fields), format.Value);
        }
        catch (CronFormatException)
        {
            return null;
        }
    }
}
