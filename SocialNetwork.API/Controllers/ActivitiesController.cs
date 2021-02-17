using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Activities.Queries;
using SocialNetwork.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    public class ActivitiesController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ActivityDto>>> GetTodoItemsWithPagination([FromQuery] GetActivitiesWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}