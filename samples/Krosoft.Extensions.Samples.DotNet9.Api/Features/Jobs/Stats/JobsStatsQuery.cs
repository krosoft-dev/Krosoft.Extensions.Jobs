using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Stats;

public record JobsStatsQuery : BaseQuery<SystemStatistics>;