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
using Infrastructure.Abstractions;

namespace Host.Controllers
{
    public class DeveloperController : BaseConroller
    {
        private readonly IDeveloperRepository Repo;

        public DeveloperController(IDeveloperRepository devRepo)
        {
            this.Repo = devRepo;
        }

        #region API

        //Get api/developer
        [HttpGet]
        public async Task<IActionResult> Get([FromBody] FilterModel filter)
        {
            if (!await ValidateFilterOrDefault(filter))
            {
                return BadRequest(ModelState);
            }

            var developers = await GetSetOrAll(filter);

            string projName = filter.Set.ToLower() == "associated" ? filter.Context : "";

            return new JsonResult(new CollectionResult<EditDeveloperViewModel>()
            {
                Values = developers.Select(x => x.GetVM(Encode(x.Nickname), projName)).ToArray(),
                TotalCount = Repo.LastQueryTotalCount
            });
        }

        //Get api/developer/Dr-Manhattan
        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            if (await Repo.Single(Decode(name)) is Developer developer)
            {
                return new JsonResult(developer.GetVM(name, null));
            }

            return NotFound();
        }

        //Post api/developer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EditDeveloperViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateModel(model))
            {
                return BadRequest(ModelState);
            }

            var developer = model.GetInstance();

            Repo.Add(developer);

            await Repo.SaveChangesAsync();

            return new JsonResult(developer.GetVM(Encode(developer.Nickname)));
        }

        //put api/developer/frekyyy-doctor
        [HttpPut("{name}")]
        public async Task<IActionResult> Put([FromRoute] string name, [FromBody] EditDeveloperViewModel model)
        {
            if (await Repo.Single(Decode(name)) is Developer original)
            {
                if (!ModelState.IsValid || !await ValidateModel(model, original))
                {
                    return BadRequest(ModelState);
                }
                //updates fields of orginal enitty
                model.Update(original);
                //sets EntityState to Modifed
                Repo.Update(original);
                // ...
                await Repo.SaveChangesAsync();
                // returns updated entity
                return new JsonResult(original.GetVM(Encode(original.Nickname)));
            }

            return NotFound();
        }

        #endregion

        protected virtual async Task<bool> ValidateModel(EditDeveloperViewModel model, Developer original = null)
        {
            if ((original != null || model.Nickname != model.Nickname) && await Repo.Exist(model.Nickname))
            {
                ModelState.AddModelError(nameof(model.Nickname), String.Format("Developer with nickname {0} already exists", model.Nickname));
            }
            if (Regex.IsMatch(model.Nickname, "-"))
            {
                ModelState.AddModelError(nameof(model.Nickname), String.Format("Developer nickname should not contain \"-\" (dash) character"));
            }

            return ModelState.IsValid;
        }

        protected virtual Task<IEnumerable<Developer>> GetSetOrAll(FilterModel filter)
        {
            if (filter.Set.ToLower() == "associated")
            {
                return Repo.GetAssignedTo(Decode(filter.Context), filter.Keywords, filter.GetOrderModel());
            }

            if (filter.Set.ToLower() == "nonassociated")
            {
                return Repo.GetAssignableTo(Decode(filter.Context), filter.Keywords, filter.GetOrderModel());
            }

            return Repo.Get(filter.Keywords, filter.GetOrderModel());
        }

        protected virtual async Task<bool> ValidateFilterOrDefault(FilterModel filter)
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
            // not sure where it goes
            //else if (!await _mng.ProjectExists(Decode(filter.Context)))  
            //{
            //    ModelState.AddModelError(nameof(filter.Context), string.Format("Project with name {0} was not found", Decode(filter.Context)));
            //}

            return ModelState.IsValid;
        }
    }
}
