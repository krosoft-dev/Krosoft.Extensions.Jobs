using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
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
}
