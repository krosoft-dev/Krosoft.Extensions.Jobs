using System.Text.Json;
using Hangfire.Common;
#if NET9_0_OR_GREATER
using Hangfire;

#else
#endif

namespace Krosoft.Extensions.Jobs.Hangfire.Extensions;

public static class JobExtensions
{
    public static string GetFingerprintKey(this Job job) => $"fingerprint:{job.GetFingerprint()}";

    private static string GetFingerprint(this Job job)
    {
        if (job.Type == null || job.Method == null)
        {
            return string.Empty;
        }

        string typeName;
        string methodName;
        string parameters;

#if NET9_0_OR_GREATER
        if (job.Type == typeof(DynamicJob) && job.Args is { Count: > 0 } && job.Args[0] is DynamicJob dynamicJob)
        {
            // Résoudre le vrai type depuis le nom string
            var resolvedType = Type.GetType(dynamicJob.Type ?? string.Empty);
            typeName = resolvedType?.FullName ?? dynamicJob.Type ?? string.Empty;
            methodName = dynamicJob.Method ?? string.Empty;

            // Les args du DynamicJob sont stockés en JSON : ["\"System_SoLong\""]
            // → désérialiser pour obtenir les valeurs brutes
            parameters = DeserializeDynamicJobArgs(dynamicJob.Args);
        }
        else
        {
#endif
            typeName = job.Type.FullName ?? string.Empty;
            methodName = job.Method.Name;
            parameters = job.Args is not null ? string.Join(".", job.Args) : string.Empty;
#if NET9_0_OR_GREATER
        }
#endif

        return $"{typeName}.{methodName}.{parameters}";
    }

#if NET9_0_OR_GREATER
    private static string DeserializeDynamicJobArgs(string? args)
    {
        if (string.IsNullOrEmpty(args))
        {
            return string.Empty;
        }

        try
        {
            // args = ["\"System_SoLong\""] → désérialise le tableau JSON
            var list = JsonSerializer.Deserialize<List<object>>(args);
            if (list == null)
            {
                return string.Empty;
            }

            var parts = list.Select(item =>
            {
                var str = item.ToString() ?? string.Empty;
                // Désérialise chaque élément s'il est encore une string JSON encodée
                try
                {
                    var inner = JsonSerializer.Deserialize<string>(str);
                    return inner ?? str;
                }
                catch
                {
                    return str;
                }
            });

            return string.Join(".", parts);
        }
        catch
        {
            return args;
        }
    }
#endif
}