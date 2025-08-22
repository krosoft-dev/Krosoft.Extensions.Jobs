using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;

public record JobsQuery : BaseQuery<IEnumerable<JobDto>>;