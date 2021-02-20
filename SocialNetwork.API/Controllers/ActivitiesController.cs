using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Activities.Commands;
using SocialNetwork.Application.Activities.Queries;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    [ApiVersion("1.0")]
    public class ActivitiesController : BaseApiController
    {
        /// <summary>
        /// Get activities
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //[Authorize(Policy = "IsActivityHost")]
        [HttpGet]
        public async Task<IActionResult> GetActivitiesWithPagination([FromQuery] GetActivitiesWithPaginationQuery query)
        {
            return HandleResult(await Mediator.Send(query));
        }

        /// <summary>
        /// Get activity detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Policy = "IsActivityHost")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(int id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetailQuery { Id = id }));
        }

        /// <summary>
        /// Create a activity
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        //[Authorize(Policy = "IsActivityHost")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateActivityCommand command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Update a activity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        //[Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateActivityCommand command)
        {
            command.Id = id;
            return HandleResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a activity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivityCommand { Id = id }));
        }
    }
}