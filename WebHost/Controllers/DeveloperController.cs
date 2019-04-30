﻿using System;
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
        private readonly IDeveloperRepository _developerRepo;

        private const int DefaultTake = 20;

        public DeveloperController(IDeveloperRepository devRepo)
        {
            this._developerRepo = devRepo;
        }

        #region API

        //GET api/developer
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DeveloperFilterModel filter)
        {
            SetFilterDefaults(filter);

            if (!await ValidateFilter(filter))
            {
                return BadRequest(ModelState.GetValidationProblemDetails());
            }

            var developers = await GetSetOrAll(filter);

            string projectUrl = filter.Set.Value == DeveloperSet.Associated ? filter.ProjectContextUrl : "";

            return Ok(new PaginationCollection<EditDeveloperViewModel>()
            {
                Values = developers.Select(x => x.GetVM(Encode(x.Nickname), projectUrl)).ToArray(),
                TotalCount = _developerRepo.LastQueryTotalCount
            });
        }

        //GET api/developer/Dr-Manhattan
        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (await _developerRepo.Single(Decode(name)) is Developer developer)
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

            await _developerRepo.Add(developer);

            await _developerRepo.SaveChangesAsync();

            return new JsonResult(developer.GetVM(Encode(developer.Nickname)));
        }

        //PUT api/developer/frekyyy-doctor
        [HttpPut("{name}")]
        public async Task<IActionResult> Put([FromRoute] string name, [FromBody] EditDeveloperViewModel updateModel)
        {
            if (await _developerRepo.Single(Decode(name)) is Developer original)
            {
                if (!await ValidateModel(updateModel, original))
                {
                    return BadRequest(ModelState.GetValidationProblemDetails());
                }
                //updates fields of orginal enitty

                updateModel.Tags = updateModel.Tags.Distinct().ToArray();

                original.FullName = updateModel.FullName;

                original.Nickname = updateModel.Nickname;

                //remove 
                foreach (var devTag in original.DeveloperTags.ToArray())
                {
                    if (!updateModel.Tags.Contains(devTag.Tag.Name))
                    {
                        original.DeveloperTags.Remove(devTag);
                    }
                }
                //add new tags
                foreach (var tagName in updateModel.Tags)
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

                await _developerRepo.Update(original);
                // ...
                await _developerRepo.SaveChangesAsync();
                // returns updated entity

                return Ok(original.GetVM(Encode(original.Nickname)));
            }

            return NotFound();
        }

        //DELETE api/developer/frekkyy-doctor
        [HttpDelete("name")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            if (await _developerRepo.Single(Decode(name)) is Developer developer)
            {
                _developerRepo.Delete(developer);

                await _developerRepo.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        #endregion

        protected virtual async Task<bool> ValidateModel(EditDeveloperViewModel model, Developer original = null)
        {
            if ((original == null || model.Nickname != original.Nickname) && await _developerRepo.Exist(model.Nickname))
            {
                ModelState.AddModelError(nameof(model.Nickname), String.Format("Developer with nickname {0} already exists", model.Nickname));
            }

            if (Regex.IsMatch(model.Nickname, "-"))
            {
                ModelState.AddModelError(nameof(model.Nickname), String.Format("Developer nickname should not contain \"-\" (dash) character"));
            }

            return ModelState.IsValid;
        }

        protected virtual Task<IEnumerable<Developer>> GetSetOrAll(DeveloperFilterModel filter)
        {
            if (filter.Set.Value == DeveloperSet.Associated)
            {
                return _developerRepo.GetAssignedTo(
                    projName: Decode(filter.ProjectContextUrl),
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());
            }

            if (filter.Set.Value == DeveloperSet.NonAssociated)
            {
                return _developerRepo.GetAssignableTo(
                    projName: Decode(filter.ProjectContextUrl),
                    keywords: filter.Keywords,
                    order: filter.GetOrderModel());
            }

            return _developerRepo.Get(filter.Keywords, filter.GetOrderModel());
        }

        protected virtual void SetFilterDefaults(DeveloperFilterModel filter)
        {
            if (!filter.Set.HasValue)
            {
                filter.Set = DeveloperSet.All;
            }

            if (!filter.Sort.HasValue)
            {
                filter.Sort = DeveloperSort.FullName;
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

            if (filter.Set.Value != DeveloperSet.Associated && filter.Set.Value != DeveloperSet.NonAssociated)
            {
                filter.ProjectContextUrl = "";
            }
        }

        protected virtual Task<bool> ValidateFilter(DeveloperFilterModel filter)
        {
            //if (!filter.Set.HasValue)
            //{
            //    ModelState.AddModelError(nameof(filter.Set), String.Format("Unrecognizable set was requested \"{0}\"", filter.Set));
            //}

            //if (!filter.Sort.HasValue)
            //{
            //    ModelState.AddModelError(nameof(filter.Set), String.Format("Unrecognizable sort column was requested \"{0}\"", filter.Sort));
            //}

            //if retriving associated data (devs of project) and context not given or project does't exist

            if ((filter.Set.Value == DeveloperSet.Associated || filter.Set.Value == DeveloperSet.NonAssociated) && string.IsNullOrEmpty(filter.ProjectContextUrl))
            {
                ModelState.AddModelError(nameof(filter.ProjectContextUrl), string.Format("Project url not provided in \"{0}\" field of request object", nameof(filter.ProjectContextUrl)));
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
