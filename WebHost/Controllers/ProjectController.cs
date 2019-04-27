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
using Infrastructure.Abstractions;

namespace Host.Controllers
{
    public class ProjectController : BaseConroller
    {
        private readonly IProjectRepository Repo;

        public ProjectController(IProjectRepository repo)
        {
            this.Repo = repo;
        }


        // api/project
        #region API
        //POST api/projects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EditProjectViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateProject(model))
            {
                return BadRequest(ModelState);
            }

            var project = model.GetInstance();

            Repo.Add(project);

            await Repo.SaveChangesAsync();

            return new JsonResult(project.GetVM(url: Encode(project.Name)));
        }

        //PUT api/projects/TestProject
        [HttpPut("{name}")]
        public async Task<IActionResult> Update([FromRoute] string name, [FromBody] EditProjectViewModel model)
        {
            if (await Repo.Single(Decode(name)) is Project original)
            {
                if (!ModelState.IsValid || !await ValidateProject(model, original))
                {
                    return BadRequest(ModelState);
                }

                original.Name = model.Name;
                original.Description = model.Description;
                original.StartDate = model.StartDate.HasValue ? model.StartDate.Value : original.StartDate;
                original.EndDate = model.EndDate.HasValue ? model.EndDate.Value : original.EndDate;
                original.Status = model.Status.HasValue ? model.Status.Value : original.Status;

                Repo.Update(original);

                await Repo.SaveChangesAsync();

                return new JsonResult(original.GetVM(Encode(original.Name)));
            }

            return NotFound();
        }

        // GET api/projects/TestProject
        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (await Repo.Single(Decode(name)) is Project project)
            {
                return new JsonResult(project.GetVM(name));
            }

            return NotFound();
        }

        // DELETE api/projects/TestProject
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            if (await Repo.Single(Decode(name)) is Project project)
            {
                Repo.Delete(project);

                await Repo.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        // GET api/projects
        [HttpGet]
        public async Task<IActionResult> Get([FromBody] FilterModel filter)
        {
            if (!await ValidateFilterOrDefault(filter))
            {
                return BadRequest(filter);
            }

            var projects = await GetSetOrAll(filter);

            return new JsonResult(new CollectionResult<EditProjectViewModel>()
            {
                Values = projects.Select(x => x.GetVM(Encode(x.Name), filter.Context)).ToArray(),
                TotalCount = Repo.LastQueryTotalCount
            });
        }

        #endregion


        //retrive predefined sets of Projects
        protected virtual Task<IEnumerable<Project>> GetSetOrAll(FilterModel filter)
        {
            if (filter.Set.ToLower() == "associated")
                return Repo.GetAssignedTo(
                    devName: filter.Context,
                    status: null,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.ToLower() == "nonassociated")
                return Repo.GetAssignableTo(
                    devName: filter.Context,
                    status: null,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.ToLower() == "completed")
                return Repo.GetByStatus(
                    status: ProjectStatus.Completed,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.ToLower() == "unstarted")
                return Repo.GetByStatus(
                    status: ProjectStatus.UnStarted,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.ToLower() == "active")
                return Repo.GetByStatus(
                    status: ProjectStatus.InProgress,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            return Repo.Get(filter.Keywords, filter.GetOrderModel());
        }


        protected virtual async Task<bool> ValidateProject(EditProjectViewModel model, Project original = null)
        {
            if (Regex.IsMatch(model.Name, "-"))
            {
                ModelState.AddModelError(nameof(original.Name), String.Format("Project name should not contain \"-\" (dash) character", model.Name));
            }

            if ((original == null || original.Name != model.Name) && await Repo.Exist(model.Name))
            {
                ModelState.AddModelError(nameof(original.Name), String.Format("Projec with name {0} already exists", model.Name));
            }

            if (model.StartDate >= model.EndDate)
            {
                ModelState.AddModelError(nameof(original.EndDate), "Project end date should be greater than start date");
            }

            return ModelState.IsValid;
        }

        private async Task<bool> ValidateFilterOrDefault(FilterModel filter)
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
                //else if (!await _mng.DeveloperExists(Decode(filter.Context)))
                //{
                //    ModelState.AddModelError(nameof(filter.Context), string.Format("Developer with nickname {0} was not found", Decode(filter.Context)));
                //}
            return ModelState.IsValid;
        }
    }
}
