using Hangfire.Common;
#if NET9_0_OR_GREATER
using Hangfire;
using System.Text.Json;
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

#if NET9_0_OR_GREATER
        if (job.Type == typeof(DynamicJob) && job.Args is { Count: > 0 } && job.Args[0] is DynamicJob dynamicJob)
        {
            return GetDynamicJobFingerprint(dynamicJob);
        }
#endif

        return GetStandardJobFingerprint(job);
    }

    private static string GetStandardJobFingerprint(Job job)
    {
        var typeName = job.Type.FullName ?? string.Empty;
        var methodName = job.Method.Name;
        var parameters = job.Args is not null ? string.Join(".", job.Args) : string.Empty;

        return $"{typeName}.{methodName}.{parameters}";
    }

#if NET9_0_OR_GREATER
    private static string GetDynamicJobFingerprint(DynamicJob dynamicJob)
    {
        var resolvedType = Type.GetType(dynamicJob.Type ?? string.Empty);
        var typeName = resolvedType?.FullName ?? dynamicJob.Type ?? string.Empty;
        var methodName = dynamicJob.Method ?? string.Empty;
        var parameters = DeserializeDynamicJobArgs(dynamicJob.Args);

        return $"{typeName}.{methodName}.{parameters}";
    }

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