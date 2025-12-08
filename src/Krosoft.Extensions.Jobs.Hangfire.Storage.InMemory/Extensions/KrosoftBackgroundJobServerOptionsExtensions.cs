using Hangfire;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Providers;

namespace Krosoft.Extensions.Jobs.Hangfire.Storage.InMemory.Extensions;

public static class KrosoftBackgroundJobServerOptionsExtensions
{
    public static BackgroundJobServerOptions UseInMemoryStorage(this KrosoftBackgroundJobServerOptions options)
    {
        options.StorageProvider = new InMemoryStorageProvider();
        return options;
    }
}