using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;

namespace Host.Controllers
{
    public class someclass

    {
        public string name { get; set; }
        public string password { get; set; }
    }
    [Route("developers")]
    public class SampleDataController : Controller
    {
        private readonly ProjectManagerService _projectManager;
        public SampleDataController(ProjectManagerService projectManager)
        {
            this._projectManager = projectManager;
        }
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [Route("create")]
        public IActionResult AddDeveloper([FromBody] EditDeveloperViewModel developer)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newDev = developer.GetInstance();
            if (_projectManager.DeveloperExists(newDev))
            {
                //addmodel error
            }
            _projectManager.Add(newDev);
            _projectManager.SaveChanges();
            return Ok();
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
