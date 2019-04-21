using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using AutoMapper.Configuration;
using Infrastructure.Services;
using System.Linq;
using Infrastructure.Entities;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Host.Controllers
{
    public class ProjectController : BaseConroller
    {
        private Dictionary<string, Func<FilterModel, Task<IEnumerable<Project>>>> Sets;
        public ProjectController(ProjectManagerService projectManager) : base(projectManager)
        {
            this.Sets = new Dictionary<string, Func<FilterModel, Task<IEnumerable<Project>>>>();
            Sets.Add("assignedto", GetAssigneProjects);
        }

        protected Task<IEnumerable<Project>> GetAssigneProjects(FilterModel filter)
        {
            return _mng.GetAssignedProjects(Decode(filter.SetContext));
        }

        public async Task<IActionResult> Create([FromBody] EditProjectViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateProject(model)) return BadRequest(ModelState);
            var project = model.GetInstance();
            _mng.Add(project);
            await _mng.SaveChanges();
            return new JsonResult(project.GetVM(url: Encode(project.Name)));
        }

        public async Task<IActionResult> Update([FromRoute] string name, [FromBody] EditProjectViewModel model)
        {
            if (await _mng.GetProject(Decode(name)) is Project original)
            {
                if (!ModelState.IsValid || !await ValidateProject(model, original)) return BadRequest(ModelState);
                model.Update(original);
                _mng.Update(original);
                await _mng.SaveChanges();
                return new JsonResult(original.GetVM(Encode(original.Name)));
            }
            return NotFound();
        }

        public async Task<IActionResult> Single(string name)
        {
            if (await _mng.GetProject(Decode(name)) is Project project)
            {
                return new JsonResult(project.GetVM(name));
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(string name)
        {
            if (await _mng.GetProject(Decode(name)) is Project project)
            {
                _mng.Delete(project);
                await _mng.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        public async Task<IActionResult> Get(FilterModel filter)
        {
            int count = 0;
            IEnumerable<Project> result = null;
            if (!string.IsNullOrEmpty(filter.Set) && Sets.ContainsKey(filter.Set.ToLower()))
            {
                result = await Sets[filter.Set.ToLower()](filter);
                count = result.Count();
            }
            else
            {
                count = _mng.CountProjects(filter.Keywords);
                result = await _mng.Get<Project>(
                            filter.Sort,
                            filter.Keywords,
                            filter.Sort== OrderDirection.Ascending,
                            filter.Skip.Value,
                            filter.Take.Value);
            }
            return new JsonResult(new CollectionResult<EditProjectViewModel>()
            {
                Values = result.Select(x => x.GetVM(Encode(x.Name))).ToArray(),
                TotalCount = count
            });
        }
        public async Task<IActionResult> Assign([FromBody] AssignModel model)
        {
            return await Assign(model, false);

        }
        public async Task<IActionResult> Unassign([FromBody] AssignModel model)
        {
            return await Assign(model, true);
        }
        protected async Task<IActionResult> Assign(AssignModel model, bool expectedAssign)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _mng.GetProject(Decode(model.Project)) is Project project && await _mng.GetDeveloper(Decode(model.Developer)) is Developer developer)
            {
                if (await _mng.IsAssigned(project, developer) != expectedAssign)
                {
                    //Developer ... already assign to project ...
                    //Developer ... already unassigned from project ...
                    ModelState.AddModelError("model", string.Format("Developer {1} already {3} project {0}", project.Name, developer.Nickname, expectedAssign ? "unassigned from" : "assigned to"));
                    return BadRequest(ModelState);
                }
                if (expectedAssign) await _mng.Unassign(project, developer);
                else await _mng.Assign(project, developer);
                await _mng.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        protected virtual async Task<bool> ValidateProject(EditProjectViewModel model, Project original = null)
        {
            if ((original == null || original.Name != model.Name) && await _mng.DeveloperExists(model.Name))
            {
                ModelState.AddModelError(nameof(original.Name), String.Format("Projec with name {0} already exists", model.Name));
            }
            if (model.StartDate >= model.EndDate)
            {
                ModelState.AddModelError(nameof(original.EndDate), "Project end date should be greater than start date");
            }
            return ModelState.IsValid;
        }
    }
}
