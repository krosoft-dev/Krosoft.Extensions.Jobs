using AutoMapper;
using Hangfire.Storage;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Jobs.Hangfire.Profiles;

namespace Krosoft.Extensions.Jobs.Hangfire.Tests.Profiles
{
    [TestClass]
    public class HangfireProfileTests
    {
        private readonly IMapper _mapper;

        public HangfireProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<HangfireProfile>());
            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public void Map_RecurringJobDto_To_CronJob_Should_Be_Valid()
        {
            // Arrange
            var recurringJobDto = new RecurringJobDto
            {
                Id = "job1",
                Cron = "0 0 * * *",
                NextExecution = DateTime.UtcNow,
                LastJobState = "Succeeded",
                LastExecution = DateTime.UtcNow.AddDays(-1),
                CreatedAt = DateTime.UtcNow.AddMonths(-1)
            };

            // Act
            var cronJob = _mapper.Map<CronJob>(recurringJobDto);

            // Assert
            Check.That(cronJob.Identifiant).IsEqualTo(recurringJobDto.Id);
            Check.That(cronJob.CronExpression).IsEqualTo(recurringJobDto.Cron);
            Check.That(cronJob.ProchaineExecutionDate).IsEqualTo(recurringJobDto.NextExecution);
            Check.That(cronJob.DerniereExecutionStatut).IsEqualTo(recurringJobDto.LastJobState);
            Check.That(cronJob.DerniereExecutionDate).IsEqualTo(recurringJobDto.LastExecution);
            Check.That(cronJob.CreationDate).IsEqualTo(recurringJobDto.CreatedAt);
        }
    }
}