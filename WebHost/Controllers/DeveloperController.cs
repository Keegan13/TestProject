using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using Host.Models;

namespace Host.Controllers
{
    public class DeveloperController : BaseConroller
    {
        public DeveloperController(ProjectManagerService projectManager, IConfiguration configuration) : base(projectManager, configuration)
        {

        }

        public IActionResult Single(string name)
        {
            if (_mng.GetDeveloper(Decode(name)) is Developer developer)
            {
                return new JsonResult(developer.GetVM(name));
            }
            return NotFound();
        }
        public IActionResult Get(FilterModel filter)
        {
            return Ok();
        }

        public IActionResult Create([FromBody] EditDeveloperViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorObject());

            var developer = model.GetInstance();

            if (_mng.DeveloperExists(developer))
            {
                ModelState.AddModelError(nameof(developer.Nickname), String.Format("Developer with nickname {0} already exists", developer.Nickname));
                return BadRequest(ModelState.GetErrorObject());
            }

            _mng.Add(developer);
            _mng.SaveChanges();

            return Ok();
        }
        public IActionResult Update()
        {
            return Ok();
        }
    }
}
