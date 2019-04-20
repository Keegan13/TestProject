using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Host.Extensions;
using Host.Models;

namespace Host.Controllers
{
    public class DeveloperController : BaseConroller
    {
        public DeveloperController(ProjectManagerService projectManager) : base(projectManager)
        {

        }

        public async Task<IActionResult> Single(string name)
        {
            if (await _mng.GetDeveloper(Decode(name)) is Developer developer)
            {
                return new JsonResult(developer.GetVM(name, null));
            }
            return NotFound();
        }
        public async Task<IActionResult> Get(FilterModel filter)
        {
            var count = _mng.CountDevelopers(filter.Keywords);
            var proj = string.IsNullOrEmpty(filter.Context) ? null : _mng.GetProject(Decode(filter.Context));

            var result = await _mng.Get<Developer>(
                filter.SortColumn,
                filter.Keywords,
                filter.SortOrder == OrderDirection.Ascending,
                filter.Skip.Value,
                filter.Take.Value);

            return new JsonResult(new CollectionResult<EditDeveloperViewModel>()
            {
                Values = result.Select(x => x.GetVM(Encode(x.Nickname), proj!=null&&_mng.IsAssigned(proj, x) ? Encode(proj.Name) : null)).ToArray(),
                TotalCount = count
            });
        }

    public async Task<IActionResult> Create([FromBody] EditDeveloperViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorObject());

        var developer = model.GetInstance();

        if (_mng.DeveloperExists(developer))
        {
            ModelState.AddModelError(nameof(developer.Nickname), String.Format("Developer with nickname {0} already exists", developer.Nickname));
            return BadRequest(ModelState.GetErrorObject());
        }

        _mng.Add(developer);
        await _mng.SaveChanges();

        return new JsonResult(developer.GetVM(Encode(developer.Nickname),null));
    }
    public IActionResult Update()
    {
        return Ok();
    }
}
}
