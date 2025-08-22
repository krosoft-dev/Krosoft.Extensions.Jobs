using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.JobTrigger;

internal record JobTriggerCommand(string Identifiant) : BaseCommand;

 