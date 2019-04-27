using Host.Models;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Host.Controllers
{

    public class AssignController : BaseConroller
    {
        protected readonly IProjectAssignments Assignments;
        protected readonly IProjectRepository ProjectRepo;
        protected readonly IDeveloperRepository DeveloperRepo;

        public AssignController(IProjectAssignments assign, IDeveloperRepository devs, IProjectRepository projs)
        {
            this.Assignments = assign;
            this.ProjectRepo = projs;
            this.DeveloperRepo = devs;
        }

        [HttpGet("{projName}/{devName}")]
        public async Task<IActionResult> Get(string projName, string devName)
        {
            return new JsonResult(new AssignModel
            {
                Project = projName,
                Developer = devName,
                isAssigned = await Assignments.IsAssigned(projName, devName)
            });
        }

        [HttpPost] // post for adding/ 
        public async Task<IActionResult> Post([FromBody] AssignModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await ProjectRepo.Single(Decode(model.Project));

            var developer = await DeveloperRepo.Single(Decode(model.Developer));

            if (project == null)
            {
                ModelState.AddModelError(nameof(AssignModel.Project), String.Format("Project with name {0} was not found", Decode(model.Developer)));
            }

            if (developer == null)
            {
                ModelState.AddModelError(nameof(AssignModel.Developer), String.Format("Developer with nickname {0} was not found", Decode(model.Developer)));
            }

            //if (await Assignments.IsAssigned())
            //{
            //    ModelState.AddModelError(nameof(AssignModel.isAssigned), String.Format("Already assigned"));
            //}

            if (model.isAssigned)
            {
                await Assignments.Assign(project, developer);
            }
            else
            {
                await Assignments.Unassign(project, developer);
            }

            await Assignments.SaveChangesAsync();

            return new JsonResult(model);

        }
    }
}
