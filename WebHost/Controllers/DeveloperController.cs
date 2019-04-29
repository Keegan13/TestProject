using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Entities;
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

        protected string[] Sets = new[] { "all", "associated", "nonassociated" };

        protected string[] Sorts = new[] { "fullname", "nickname" };

        public DeveloperController(IDeveloperRepository devRepo)
        {
            this.Repo = devRepo;
        }

        #region API

        //GET api/developer
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterModel filter)
        {
            if (!await ValidateFilterOrDefault(filter))
            {
                return BadRequest(ModelState);
            }

            var developers = await GetSetOrAll(filter);

            string projName = filter.Set.ToLower() == "associated" ? filter.Context : "";


            return Ok(new CollectionResult<EditDeveloperViewModel>()
            {
                Values = developers.Select(x => x.GetVM(Encode(x.Nickname), projName)).ToArray(),
                TotalCount = Repo.LastQueryTotalCount
            });
        }

        //GET api/developer/Dr-Manhattan
        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (await Repo.Single(Decode(name)) is Developer developer)
            {
                return Ok(developer.GetVM(name, null));
            }

            return NotFound();
        }

        //POST api/developer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EditDeveloperViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateModel(model))
            {
                return BadRequest(ModelState.GetValidationProblemDetails());
            }

            var developer = model.GetInstance();

            await Repo.Add(developer);

            await Repo.SaveChangesAsync();

            return new JsonResult(developer.GetVM(Encode(developer.Nickname)));
        }

        //PUT api/developer/frekyyy-doctor
        [HttpPut("{name}")]
        public async Task<IActionResult> Put([FromRoute] string name, [FromBody] EditDeveloperViewModel model)
        {
            if (await Repo.Single(Decode(name)) is Developer original)
            {
                if (!await ValidateModel(model, original))
                {
                    return BadRequest(ModelState.GetValidationProblemDetails());
                }
                //updates fields of orginal enitty

                model.Skills = model.Skills.Distinct().ToArray();

                original.FullName = model.FullName;

                original.Nickname = model.Nickname;

                //remove 
                foreach (var devTag in original.DeveloperTags.ToArray())
                {
                    if (!model.Skills.Contains(devTag.Tag.Name))
                    {
                        original.DeveloperTags.Remove(devTag);
                    }
                }
                //add new tags
                foreach (var tagName in model.Skills)
                {
                    if (original.DeveloperTags.All(x => x.Tag.Name != tagName))
                    {
                        original.DeveloperTags.Add(
                            new DeveloperTag
                            {
                                Developer = original,
                                Tag = new Tag { Name = tagName }
                            });
                    }
                }

                await Repo.Update(original);
                // ...
                await Repo.SaveChangesAsync();
                // returns updated entity

                return Ok(original.GetVM(Encode(original.Nickname)));
            }

            return NotFound();
        }

        //DELETE api/developer/frekkyy-doctor
        [HttpDelete("name")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            if (await Repo.Single(Decode(name)) is Developer developer)
            {
                Repo.Delete(developer);

                await Repo.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        #endregion

        protected virtual async Task<bool> ValidateModel(EditDeveloperViewModel model, Developer original = null)
        {
            if ((original == null || model.Nickname != original.Nickname) && await Repo.Exist(model.Nickname))
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
                return Repo.GetAssignedTo(
                    projName: Decode(filter.Context),
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());
            }

            if (filter.Set.ToLower() == "nonassociated")
            {
                return Repo.GetAssignableTo(
                    projName: Decode(filter.Context),
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());
            }

            return Repo.Get(filter.Keywords, filter.GetOrderModel());
        }

        protected virtual Task<bool> ValidateFilterOrDefault(FilterModel filter)
        {
            //seting defaults
            if (string.IsNullOrEmpty(filter.Set)) filter.Set = "all";

            if (!filter.Order.HasValue) filter.Order = OrderDirection.Ascending;

            if (string.IsNullOrEmpty(filter.Sort)) filter.Sort = "fullname";

            //adding errors for bad fields

            if (!this.Sets.Contains(filter.Set.ToLower()))
            {
                ModelState.AddModelError(nameof(filter.Set), String.Format("Unrecognizable set was requested \"{0}\"", filter.Set));
            }

            if (!this.Sorts.Contains(filter.Sort.ToLower()))
            {
                ModelState.AddModelError(nameof(filter.Set), String.Format("Unrecognizable sort column was requested \"{0}\"", filter.Sort));
            }

            //if retriving associated data (devs of project) and context not given or project does't exist
            if (filter.Set.ToLower() == "associated" && string.IsNullOrEmpty(filter.Context))
            {
                ModelState.AddModelError(nameof(filter.Context), "Project name not provided in \"Context\" field of request object");
            }


            // not sure where it goes
            //else if (!await _mng.ProjectExists(Decode(filter.Context)))  
            //{
            //    ModelState.AddModelError(nameof(filter.Context), string.Format("Project with name {0} was not found", Decode(filter.Context)));
            //}

            return Task.FromResult<bool>(ModelState.IsValid);
        }
    }
}
