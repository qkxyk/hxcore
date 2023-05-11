using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.MiddleWares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAuthorizationService authorizationService)
        {
            _logger = logger;
            this._authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAsync()
        {
            _logger.LogInformation("这个是一般的日志");
            _logger.LogWarning("这个是警告日志");
            _logger.LogError("这个是错误日志");
            var req = new ModuleRequirement(1);
            ResourceData resource = new ResourceData { Compare = CompareData.Equal, Operate = 1, ProjectId = 1024 };
            var t = await _authorizationService.AuthorizeAsync(User, resource, req);
            if (t.Succeeded)
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
            else
            {
                return new ForbidResult();
            }
        }

        [HttpGet("Image")]
        public ActionResult GetImage()
        {
            string image = @"Image/057a556af24f494094a2586fbe9335a4/TypeImage/3/20200821043629.jpg";
            return Ok(image);
        }
    }
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
