using Hangfire;
using Hangfire.Common;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Tests.Extensions;

[TestClass]
public class JobExtensionsTests
{
    [TestMethod]
    public void GetFingerprintKey_WithTypeAndMethod_ReturnsExpectedKey()
    {
        var method = typeof(SampleJob).GetMethod(nameof(SampleJob.ExecuteAsync))!;
        var job = new Job(typeof(SampleJob), method, "arg1");

        var result = job.GetFingerprintKey();

        Check.That(result).IsEqualTo($"fingerprint:{typeof(SampleJob).FullName}.ExecuteAsync.arg1");
    }

    [TestMethod]
    public void GetFingerprintKey_WithSingleArg_ReturnsExpectedKey()
    {
        var method = typeof(SampleJob).GetMethod(nameof(SampleJob.ExecuteAsync))!;
        var job = new Job(typeof(SampleJob), method, "myArg");

        var result = job.GetFingerprintKey();

        Check.That(result).IsEqualTo($"fingerprint:{typeof(SampleJob).FullName}.ExecuteAsync.myArg");
    }

    [TestMethod]
    public void GetFingerprintKey_DynamicJob_ResolvesRealType()
    {
        var dynamicJob = CreateDynamicJob(typeof(SampleJob).AssemblyQualifiedName!,
                                          "Execute",
                                          "[\"\\\"value1\\\"\"]");

        var method = typeof(DynamicJob).GetMethod(nameof(Equals))!;
        var job = new Job(typeof(DynamicJob), method, dynamicJob);

        var result = job.GetFingerprintKey();

        Check.That(result).StartsWith("fingerprint:");
        Check.That(result).Contains(typeof(SampleJob).FullName!);
        Check.That(result).Contains("Execute");
        Check.That(result).Contains("value1");
    }

    [TestMethod]
    public void GetFingerprintKey_DynamicJob_NullArgs_ReturnsKeyWithEmptyArgs()
    {
        var dynamicJob = CreateDynamicJob(typeof(SampleJob).AssemblyQualifiedName!,
                                          "Execute",
                                          null!);

        var method = typeof(DynamicJob).GetMethod(nameof(Equals))!;
        var job = new Job(typeof(DynamicJob), method, dynamicJob);

        var result = job.GetFingerprintKey();

        Check.That(result).StartsWith("fingerprint:");
        Check.That(result).Contains("Execute");
    }

    [TestMethod]
    public void GetFingerprintKey_DynamicJob_EmptyArgs_ReturnsKeyWithEmptyArgs()
    {
        var dynamicJob = CreateDynamicJob(typeof(SampleJob).AssemblyQualifiedName!,
                                          "Execute",
                                          "");

        var method = typeof(DynamicJob).GetMethod(nameof(Equals))!;
        var job = new Job(typeof(DynamicJob), method, dynamicJob);

        var result = job.GetFingerprintKey();

        Check.That(result).StartsWith("fingerprint:");
    }

    private static DynamicJob CreateDynamicJob(string type, string method, string args) => new(type, method, "", args, Array.Empty<JobFilterAttribute>(), "");

    internal class SampleJob : IRecurringJob
    {
        public string Type => nameof(SampleJob);
        public Task<JobResult> ExecuteAsync(string identifiant) => throw new NotImplementedException();
    }
}