using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Jobs.Jobs;

public record JobsQuery : BaseQuery<IEnumerable<JobDto>>;