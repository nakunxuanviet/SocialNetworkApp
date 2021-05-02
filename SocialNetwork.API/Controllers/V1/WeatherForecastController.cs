using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Activities.Queries;
using SocialNetwork.Domain.Entities.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : BaseApiController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// Get list weather forecast
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [MapToApiVersion("1.0")]
        [HttpGet("redis-get-list-sample")]
        public async Task<ActionResult<List<Activity>>> GetAllActivitiesUsingRedisCache([FromQuery] GetAllActivitieslWithRedisCacheQuery query)
        {
            return await _mediator.Send(query);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("redis-get-single-sample")]
        public async Task<ActionResult<Activity>> GetActivityByIdUsingRedisCache([FromQuery] GetActivityDetailWithRedisQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}