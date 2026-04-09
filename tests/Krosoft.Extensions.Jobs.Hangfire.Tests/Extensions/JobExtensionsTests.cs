using Hangfire;
using Hangfire.Common;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;

namespace Krosoft.Extensions.Jobs.Hangfire.Tests.Extensions;

[TestClass]
public class JobExtensionsTests
{
    [TestMethod]
    public void GetFingerprintKey_WithTypeAndMethod_ReturnsExpectedKey()
    {
        var method = typeof(SampleJobService).GetMethod(nameof(SampleJobService.Execute))!;
        var job = new Job(typeof(SampleJobService), method, "arg1", "arg2");

        var result = job.GetFingerprintKey();

        Check.That(result).IsEqualTo($"fingerprint:{typeof(SampleJobService).FullName}.Execute.arg1.arg2");
    }

    [TestMethod]
    public void GetFingerprintKey_WithSingleArg_ReturnsExpectedKey()
    {
        var method = typeof(SampleJobService).GetMethod(nameof(SampleJobService.Run))!;
        var job = new Job(typeof(SampleJobService), method, "myArg");

        var result = job.GetFingerprintKey();

        Check.That(result).IsEqualTo($"fingerprint:{typeof(SampleJobService).FullName}.Run.myArg");
    }

    [TestMethod]
    public void GetFingerprintKey_DynamicJob_ResolvesRealType()
    {
        var dynamicJob = CreateDynamicJob(typeof(SampleJobService).AssemblyQualifiedName!,
                                          "Execute",
                                          "[\"\\\"value1\\\"\"]");

        var method = typeof(DynamicJob).GetMethod(nameof(Equals))!;
        var job = new Job(typeof(DynamicJob), method, dynamicJob);

        var result = job.GetFingerprintKey();

        Check.That(result).StartsWith("fingerprint:");
        Check.That(result).Contains(typeof(SampleJobService).FullName!);
        Check.That(result).Contains("Execute");
        Check.That(result).Contains("value1");
    }

    [TestMethod]
    public void GetFingerprintKey_DynamicJob_NullArgs_ReturnsKeyWithEmptyArgs()
    {
        var dynamicJob = CreateDynamicJob(typeof(SampleJobService).AssemblyQualifiedName!,
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
        var dynamicJob = CreateDynamicJob(typeof(SampleJobService).AssemblyQualifiedName!,
                                          "Execute",
                                          "");

        var method = typeof(DynamicJob).GetMethod(nameof(Equals))!;
        var job = new Job(typeof(DynamicJob), method, dynamicJob);

        var result = job.GetFingerprintKey();

        Check.That(result).StartsWith("fingerprint:");
    }

    private static DynamicJob CreateDynamicJob(string type, string method, string args) => new(type, method, "", args, Array.Empty<JobFilterAttribute>(), "");

    public class SampleJobService
    {
        public void Execute(string arg1, string arg2)
        {
        }

        public void Run(string arg)
        {
        }
    }
}