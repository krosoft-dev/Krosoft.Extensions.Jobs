using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Jobs.Stats;

public record JobsStatsQuery : BaseQuery<SystemStatistics>;