using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using Host.Models;
using System.Text.RegularExpressions;

namespace Host.Controllers
{
    public class DeveloperController : BaseConroller<Developer>
    {
        protected override void InitializeSets(Dictionary<string, Func<FilterModel, Task<IEnumerable<Developer>>>> sets)
        {
            sets.Add("associated", GetByProject);
            sets.Add("nonassociated", GetNotByProject);
            sets.Add("all", GetAll);
        }


        protected override Task<IEnumerable<Developer>> DefaultSet(FilterModel filter)
        {
            return this.GetAll(filter);
        }

        public DeveloperController(ProjectManagerService projectManager) : base(projectManager)
        {

        }

        private Task<IEnumerable<Developer>> GetByProject(FilterModel filter)
        {
            return _mng.GetAssignedDevelopers(Decode(filter.Context),filter.Keywords, filter.GetOrderModel());
        }

        private Task<IEnumerable<Developer>> GetAll(FilterModel filter)
        {
            return _mng.Get<Developer>(filter.Keywords, filter.GetOrderModel());
        }

        private Task<IEnumerable<Developer>> GetNotByProject(FilterModel filter)
        {
            return _mng.GetNotAssignedDevelopers(Decode(filter.Context), filter.Keywords, filter.GetOrderModel());
        }

        private async Task<bool> ValidateFilterOrDefault(FilterModel filter)
        {
            if (string.IsNullOrEmpty(filter.Set)) filter.Set = "all";
            //if order not provided use ascending
            if (!filter.Order.HasValue) filter.Order = OrderDirection.Ascending;
            //if no sort Column  given use fullName
            if (string.IsNullOrEmpty(filter.Sort)) filter.Sort = "fullname";
            //if retriving associated data (devs of project) and context not given or project does't exist
            if (filter.Set.ToLower() == "associated")
                if (string.IsNullOrEmpty(filter.Context))
                {
                    ModelState.AddModelError(nameof(filter.Context), "Project name not provided in \"Context\" field");
                }
                else if (!await _mng.ProjectExists(Decode(filter.Context)))
                {
                    ModelState.AddModelError(nameof(filter.Context), string.Format("Project with name {0} was not found", Decode(filter.Context)));
                }

            return ModelState.IsValid;
        }

        public async Task<IActionResult> Get(FilterModel filter)
        {
            if (!await ValidateFilterOrDefault(filter)) return BadRequest(ModelState);

            var developers = await Set(filter);
            var count = _mng.LastQueryTotalCount;
            string projName = filter.Set.ToLower() == "associated" ? filter.Context : "";
            return new JsonResult(new CollectionResult<EditDeveloperViewModel>()
            {
                Values = developers.Select(x => x.GetVM(Encode(x.Nickname), projName)).ToArray(),
                TotalCount = count
            });
        }

        public async Task<IActionResult> Single(string name)
        {
            if (await _mng.Single(Decode(name)) is Developer developer)
            {
                return new JsonResult(developer.GetVM(name, null));
            }
            return NotFound();
        }

        public async Task<IActionResult> Create([FromBody] EditDeveloperViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateModel(model)) return BadRequest(ModelState);

            var developer = model.GetInstance();

            _mng.Add(developer);

            await _mng.SaveChanges();

            return new JsonResult(developer.GetVM(Encode(developer.Nickname), null));
        }

        protected virtual async Task<bool> ValidateModel(EditDeveloperViewModel model, Developer original = null)
        {
            if ((original != null || model.Nickname != model.Nickname) && await _mng.DeveloperExists(model.Nickname))
            {
                ModelState.AddModelError(nameof(model.Nickname), String.Format("Developer with nickname {0} already exists", model.Nickname));
            }
            if (Regex.IsMatch(model.Nickname, "-"))
            {
                ModelState.AddModelError(nameof(model.Nickname), String.Format("Developer nickname should not contain \"-\" (dash) character"));
            }

            return ModelState.IsValid;
        }

        
        public async Task<IActionResult> Update([FromRoute] string name, [FromBody] EditDeveloperViewModel model)
        {
            if (await _mng.Single(Decode(name)) is Developer original)
            {
                if (!ModelState.IsValid||!await ValidateModel(model,original)) return BadRequest(ModelState);
                model.Update(original);
                _mng.Update(original);
                await _mng.SaveChanges();

                return new JsonResult(original.GetVM(Encode(original.Nickname), null));
            }

            return NotFound();
        }


    }
}
