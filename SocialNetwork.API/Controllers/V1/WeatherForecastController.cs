using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Activities.Queries;
using SocialNetwork.Domain.Entities.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// Get activity detail using caching with MediatR
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Policy = "IsActivityHost")]
        [MapToApiVersion("1.0")]
        [HttpGet("caching-with-mediatR/{id}")]
        [ProducesResponseType(typeof(ActivityDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityWithCacheMediatR(int id)
        {
            return HandleResult(await _mediator.Send(new GetActivityQueryCachingWithMediatR { Id = id, BypassCache = false }));
        }
    }
}