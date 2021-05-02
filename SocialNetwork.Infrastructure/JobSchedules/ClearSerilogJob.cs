using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace SocialNetwork.Infrastructure.JobSchedules
{
    public class ClearSerilogJob
    {
        private readonly ILogger<ClearSerilogJob> _logger;
        private readonly IConfiguration _configuration;

        public ClearSerilogJob(ILogger<ClearSerilogJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Run()
        {
            _logger.LogInformation("Clear log job starts...");

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var durations = Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:ClearLogDuration"));
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                //var queryString = $"DELETE TOP (1000) FROM [dbo].[Logs] WHERE TimeStamp < DATEADD(day, -{durations}, GETUTCDATE())";
                var queryString = $"TRUNCATE TABLE [dbo].[Logs];";
                await connection.ExecuteAsync(queryString);
            }
            _logger.LogInformation("Clear log job completed");
        }
    }
}