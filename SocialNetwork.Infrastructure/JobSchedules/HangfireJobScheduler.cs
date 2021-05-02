using Hangfire;
using Microsoft.Extensions.Configuration;

namespace SocialNetwork.Infrastructure.JobSchedules
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringClearLogJob(IConfiguration configuration)
        {
            var cronEx = configuration.GetValue<string>("ScheduleJob:ClearSerilogCron"); ;
            RecurringJob.AddOrUpdate<ClearSerilogJob>(job => job.Run(), cronExpression: cronEx);
        }
    }
}