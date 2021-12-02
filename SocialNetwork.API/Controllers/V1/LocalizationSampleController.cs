using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class LocalizationSampleController : BaseApiController
    {
        private readonly ILogger<LocalizationSampleController> _logger;
        private readonly IStringLocalizer<LocalizationSampleController> _loc;
        public LocalizationSampleController(ILogger<LocalizationSampleController> logger, IStringLocalizer<LocalizationSampleController> loc)
        {
            _logger = logger;
            _loc = loc;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation(_loc["hi"]);
            var message = _loc["hi"].ToString();
            return Ok(message);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var message = string.Format(_loc["welcome"], name);
            return Ok(message);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var message = _loc.GetAllStrings();
            return Ok(message);
        }
    }
}
