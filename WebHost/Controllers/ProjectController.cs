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
using System.Text.RegularExpressions;

namespace Host.Controllers
{
    public class ProjectController : BaseConroller<Project>
    {
        protected override void InitializeSets(Dictionary<string, Func<FilterModel, Task<IEnumerable<Project>>>> sets)
        {
            sets.Add("associated", GetAssociated);
            sets.Add("all", GetAll);
            sets.Add("active", GetActive);
            sets.Add("upcomming", GetUpcomming);
            sets.Add("completed", GetCompleted);
            sets.Add("nonassociated", GetNonAssociated);
        }

        private Task<IEnumerable<Project>> GetNonAssociated(FilterModel filter)
        {
            return _mng.GetNotAssignedProjects(filter.Context,filter.Keywords,filter.GetOrderModel());
        }

        protected override Task<IEnumerable<Project>> DefaultSet(FilterModel filter)
        {
            return this.GetAll(filter);
        }

        public ProjectController(ProjectManagerService projectManager) : base(projectManager)
        {

        }

        protected Task<IEnumerable<Project>> GetAll(FilterModel filter)
        {
            return _mng.Get<Project>(filter.Keywords, filter.GetOrderModel());
        }
        protected Task<IEnumerable<Project>> GetActive(FilterModel filter)
        {
            return _mng.GetByStatus(ProjectStatus.InProgress, filter.Keywords ,filter.GetOrderModel());
        }
        protected Task<IEnumerable<Project>> GetAssociated(FilterModel filter)
        {
            return _mng.GetAssignedProjects(Decode(filter.Context),filter.GetOrderModel());
        }

        protected Task<IEnumerable<Project>> GetUpcomming(FilterModel filter)
        {
            return _mng.GetByStatus(ProjectStatus.UnStarted,filter.Keywords, filter.GetOrderModel());
        }
        protected Task<IEnumerable<Project>> GetCompleted(FilterModel filter)
        {
            return _mng.GetByStatus(ProjectStatus.Completed, filter.Keywords, filter.GetOrderModel());
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
        private async Task<bool> ValidateOrDefault(FilterModel filter)
        {
            if (!filter.Order.HasValue) filter.Order = OrderDirection.Ascending;
            if (string.IsNullOrEmpty(filter.Set)) filter.Set = "all";
            //if no sort Column  given use fullName
            if (string.IsNullOrEmpty(filter.Sort)) filter.Sort = "name";
            //if retriving associated data (devs of project) and context not given or project does't exist
            if (filter.Set.ToLower() == "associated")
                if (string.IsNullOrEmpty(filter.Context))
                {
                    ModelState.AddModelError(nameof(filter.Context), "Developer url not provided in \"Context\" field");
                }
                else if (!await _mng.DeveloperExists(Decode(filter.Context)))
                {
                    ModelState.AddModelError(nameof(filter.Context), string.Format("Developer with nickname {0} was not found", Decode(filter.Context)));
                }
            return ModelState.IsValid;
        }


        public async Task<IActionResult> Get(FilterModel filter)
        {
            if (!await ValidateOrDefault(filter)) return BadRequest(filter);

            var projects = await Set(filter);

            var count = _mng.LastQueryTotalCount;

            return new JsonResult(new CollectionResult<EditProjectViewModel>()
            {
                Values = projects.Select(x => x.GetVM(Encode(x.Name))).ToArray(),
                TotalCount = count
            });
        }
        public async Task<IActionResult> Assign([FromBody] AssignModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _mng.GetProject(Decode(model.Project)) is Project project && await _mng.GetDeveloper(Decode(model.Developer)) is Developer developer)
            {
                if (await _mng.IsAssigned(project, developer) != model.isAssigned)
                {
                    if(model.isAssigned) await _mng.Assign(project, developer);
                    else  await _mng.Unassign(project, developer);
                    await _mng.SaveChanges();
                    ////Developer ... already assign to project ...
                    ////Developer ... already unassigned from project ...
                    //ModelState.AddModelError("model", string.Format("Developer {1} already {2} project {0}", project.Name, developer.Nickname, !model.isAssigned ? "unassigned from" : "assigned to"));
                    //return BadRequest(ModelState);
                }
                return new JsonResult(model);
            }
            return NotFound();
        }
        protected virtual async Task<bool> ValidateProject(EditProjectViewModel model, Project original = null)
        {
            if (Regex.IsMatch(model.Name, "-"))
            {
                ModelState.AddModelError(nameof(original.Name), String.Format("Project name should not contain \"-\" (dash) character", model.Name));
            }

            if ((original == null || original.Name != model.Name) &&  await _mng.ProjectExists(model.Name))
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
