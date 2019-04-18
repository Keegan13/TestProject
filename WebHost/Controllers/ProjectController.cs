using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using AutoMapper.Configuration;
using Infrastructure.Services;
using System.Linq;
using Infrastructure.Entities;
using System;

namespace Host.Controllers
{
    public class ProjectController : BaseConroller
    {
        public ProjectController(ProjectManagerService projectManager, IConfiguration configuration) : base(projectManager, configuration)
        {

        }

        public IActionResult Create([FromBody] EditProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorObject());

            var project = model.GetInstance();

            if (_mng.ProjectExists(project))
            {
                ModelState.AddModelError(nameof(Project.Name), String.Format("Project with name {0} already exists", project.Name));
                return BadRequest(ModelState.GetErrorObject());
            }

            _mng.Add(project);
            _mng.SaveChanges();

            return Ok();
        }
        public IActionResult Update([FromQuery] string name, [FromBody] EditProjectViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorObject());

            var original = _mng.GetProject(name);

            if (string.Compare(original.Name, model.Name, true) != 0)
            {

            }
            return Ok();
        }

        public IActionResult Single(string name)
        {
            var decodedName = Decode(name);
            if (_mng.GetProject(decodedName) is Project project)
            {
                return new JsonResult(project);
            }
            return NotFound();
        }

        public IActionResult Get(FilterModel filter)
        {

            var result = _mng.GetProjects();
            return Ok();
        }

        public IActionResult Assign([FromQuery] string name, [FromQuery] string nickname)
        {
            return Ok();
        }
        public IActionResult Unassign([FromQuery] string name, [FromQuery] string nickname)
        {
            return Ok();
        }

    }
}
