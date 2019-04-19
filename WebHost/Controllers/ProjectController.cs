using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using AutoMapper.Configuration;
using Infrastructure.Services;
using System.Linq;
using Infrastructure.Entities;
using System;
using System.Threading.Tasks;

namespace Host.Controllers
{
    public class ProjectController : BaseConroller
    {
        public ProjectController(ProjectManagerService projectManager) : base(projectManager)
        {

        }

        public async Task<IActionResult> Create([FromBody] EditProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorObject());

            var project = model.GetInstance();

            if (await _mng.ProjectExists(project))
            {
                ModelState.AddModelError(nameof(Project.Name), String.Format("Project with name {0} already exists", project.Name));
                return BadRequest(ModelState.GetErrorObject());
            }

            _mng.Add(project);
            await _mng.SaveChanges();
            return new JsonResult(project.GetVM(Encode(project.Name)));
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
            name = Decode(name);
            if (_mng.GetProject(name) is Project project)
            {
                return new JsonResult(project);
            }
            return NotFound();
        }

        public async Task<IActionResult> Get(FilterModel filter)
        {
            var count = _mng.CountProjects(filter.Keywords);
            var result = await _mng.GetProjects(
                filter.SortColumn,
                filter.Keywords,
                filter.SortOrder == OrderDirection.Ascending,
                filter.Skip.Value,
                filter.Take.Value);
            return new JsonResult(new CollectionResult<EditProjectViewModel>()
            {
                Values = result.Select(x => x.GetVM(Encode(x.Name))).ToArray(),
                TotalCount = count
            });
        }

        public IActionResult GetActive(int? skip, int? take)
        {
            return new JsonResult(_mng.GetActiveProjects(skip.Value, take.Value).Select(x => x.GetVM(Encode(x.Name))).ToArray());
        }

        public async Task<IActionResult> Assign([FromQuery] string name, [FromQuery] string nickname)
        {
            name = Decode(name);
            nickname = Decode(nickname);
            if (_mng.GetProject(name) is Project project && await _mng.GetDeveloper(nickname) is Developer developer)
            {
                if (!_mng.IsAssigned(project, developer))
                {
                    _mng.AssignDeveloper(project, developer);
                    await _mng.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            return NotFound();
        }
        public async Task<IActionResult> Unassign([FromQuery] string name, [FromQuery] string nickname)
        {
            name = Decode(name);
            nickname = Decode(nickname);
            if (_mng.GetProject(name) is Project project && await _mng.GetDeveloper(nickname) is Developer developer)
            {
                if (_mng.IsAssigned(project, developer))
                {
                    _mng.DismissDeveloper(project, developer);
                    await _mng.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            return NotFound();
        }

    }
}
