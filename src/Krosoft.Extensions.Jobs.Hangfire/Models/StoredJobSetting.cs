namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public record StoredJobSetting : JobAutomatiqueSetting
{
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
