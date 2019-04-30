using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using AutoMapper.Configuration;
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
        protected const int DefaultTake = 25;

        private readonly IProjectRepository Repo;

        public ProjectController(IProjectRepository repo)
        {
            this.Repo = repo;
        }


        // api/project
        #region API
        //POST api/project
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EditProjectViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateProject(model))
            {
                return BadRequest(ModelState.GetValidationProblemDetails());
            }

            var project = model.GetInstance();

            Repo.Add(project);

            await Repo.SaveChangesAsync();

            return new JsonResult(project.GetVM(url: Encode(project.Name)));
        }

        //PUT api/project/TestProject
        [HttpPut("{name}")]
        public async Task<IActionResult> Put([FromRoute] string name, [FromBody] EditProjectViewModel model)
        {
            if (await Repo.Single(Decode(name)) is Project original)
            {
                if (!ModelState.IsValid || !await ValidateProject(model, original))
                {
                    return BadRequest(ModelState.GetValidationProblemDetails());
                }

                original.Name = model.Name;
                original.Description = model.Description;
                original.StartDate = model.StartDate;
                original.EndDate = model.EndDate;
                original.Status = model.Status;

                Repo.Update(original);

                await Repo.SaveChangesAsync();

                return new JsonResult(original.GetVM(Encode(original.Name)));
            }

            return NotFound();
        }

        // GET api/project/TestProject
        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (await Repo.Single(Decode(name)) is Project project)
            {
                return new JsonResult(project.GetVM(name));
            }

            return NotFound();
        }

        // DELETE api/project/TestProject
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

        // GET api/project
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProjectFilterModel filter)
        {
            SetFilterDefaults(filter);

            if (!await ValidateFilter(filter))
            {
                return BadRequest(ModelState.GetValidationProblemDetails());
            }

            var projects = await GetSetOrAll(filter);

            string developerUrl = filter.Set.Value == ProjectSet.Associated ? filter.DeveloperContextUrl : "";

            return new JsonResult(new PaginationCollection<EditProjectViewModel>()
            {
                Values = projects
                .Select(x => x.GetVM(Encode(x.Name), developerUrl))
                .ToArray(),
                TotalCount = Repo.LastQueryTotalCount
            });
        }

        #endregion


        //retrive predefined sets of Projects
        protected virtual Task<IEnumerable<Project>> GetSetOrAll(ProjectFilterModel filter)
        {
            //ToDo switch statements should be omitted using polymorphism

            if (filter.Set.Value == ProjectSet.Associated)
                return Repo.GetAssignedTo(
                    devName: filter.DeveloperContextUrl,
                    status: null,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.Value == ProjectSet.NonAssociated)
                return Repo.GetAssignableTo(
                    devName: filter.DeveloperContextUrl,
                    status: null,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.Value == ProjectSet.Completed)
                return Repo.GetByStatus(
                    status: ProjectStatus.Completed,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.Value == ProjectSet.UnStarted)
                return Repo.GetByStatus(
                    status: ProjectStatus.UnStarted,
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());

            if (filter.Set.Value == ProjectSet.Active)
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

        protected virtual void SetFilterDefaults(ProjectFilterModel filter)
        {
            if (!filter.Set.HasValue)
            {
                filter.Set = ProjectSet.All;
            }

            if (!filter.Sort.HasValue)
            {
                filter.Sort = ProjectSort.Name;
            }

            if (!filter.Order.HasValue)
            {
                filter.Order = OrderDirection.Ascending;
            }

            if (!filter.Skip.HasValue || filter.Skip.Value < 0)
            {
                filter.Skip = 0;
            }

            if (!filter.Take.HasValue || filter.Take.Value < 0)
            {
                filter.Take = DefaultTake;
            }

            if (filter.Set.Value != ProjectSet.Associated && filter.Set.Value != ProjectSet.NonAssociated)
            {
                filter.DeveloperContextUrl = "";
            }
        }

        //ToDO
        private async Task<bool> ValidateFilter(ProjectFilterModel filter)
        {
            //if retriving associated data (devs of project) and context not given or project does't exist
            if (filter.Set.Value == ProjectSet.Associated || filter.Set.Value == ProjectSet.NonAssociated)
            {
                if (string.IsNullOrEmpty(filter.DeveloperContextUrl))
                {
                    ModelState.AddModelError(nameof(filter.DeveloperContextUrl), string.Format("Developer url not provided in \"Context\" field", nameof(filter.DeveloperContextUrl)));
                }
                //else if (!await _mng.DeveloperExists(Decode(filter.Context)))
                //{
                //    ModelState.AddModelError(nameof(filter.Context), string.Format("Developer with nickname {0} was not found", Decode(filter.Context)));
                //}
            }

            return ModelState.IsValid;
        }
    }
}
