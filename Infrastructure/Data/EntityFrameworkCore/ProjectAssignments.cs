using Infrastructure.Abstractions;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.EntityState;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public class ProjectAssignments : IProjectAssignments, IUnitOfWork
    {
        private readonly ApplicationContext _context;

        protected DbSet<ProjectAssignment> Assignments => this._context.Set<ProjectAssignment>();

        public ProjectAssignments(ApplicationContext context)
        {
            this._context = context;

        }

        public Task Assign(Project project, Developer developer)
        {
            Assign(project.Id, developer.Id);
            return Task.CompletedTask;
        }

        public async Task Unassign(string projName, string devNickname)
        {
            var assignment = await Assignments.SingleAsync(x => x.Project.Name == projName && x.Developer.Nickname == devNickname);
            Assignments.Remove(assignment);
        }

        public Task<bool> IsAssigned(string projName, string devNickname)
        {
            return Assignments.AnyAsync(x => x.Developer.Nickname == devNickname && x.Project.Name == projName);
        }

        public Task<int> SaveChangesAsync()
        {
            return this._context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }


        #region Trusted Inputs
        protected virtual void Assign(int projId, int devId)
        {
            var assignment = new ProjectAssignment { ProjectId = projId, DeveloperId = devId };
            Assignments.Add(assignment);
        }
        protected virtual async Task Unassign(int projId, int devId)
        {
            var assignment = await Assignments.SingleAsync(x => x.DeveloperId == devId && x.ProjectId == projId);
            Assignments.Remove(assignment);
        }

        public Task Assign(string projName, string devNickname)
        {
            throw new NotImplementedException();
        }

        public Task Unassign(Project project, Developer developer)
        {
            throw new NotImplementedException();
        }


        #endregion
    }


}
