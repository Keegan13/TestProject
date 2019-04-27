using Infrastructure.Abstractions;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public class ProjectRepository : IPaginationRepository, IProjectRepository, IUnitOfWork
    {
        private readonly ApplicationContext _context;
        protected virtual DbSet<Project> Projects => _context.Set<Project>();

        public int LastQueryTotalCount { get; protected set; }

        protected virtual OrderModel DefaultOrderModel => new OrderModel
        {
            SortColumn = "Name",
            isAscendingOrder = true
        };

        public ProjectRepository(ApplicationContext context)
        {
            this._context = context;
            this.LastQueryTotalCount = 0;
        }

        public Task<Project> Single(string name)
        {
            return Projects.SingleOrDefaultAsync(x => x.Name == name);
        }

        public void Delete(Project project)
        {
            Projects.Remove(project);
        }

        public void Add(Project project)
        {
            Projects.Add(project);
        }

        public Task<bool> Exist(string projName)
        {
            return Projects.AnyAsync(x => x.Name == projName);
        }

        public Task<bool> Exist(int id)
        {
            return Projects.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Project>> Get(string keywords = null, OrderModel order = null)
        {
            var query = Projects.Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Project>> GetByStatus(ProjectStatus status, string keywords = null, OrderModel order = null)
        {
            var query = Projects.Where(x => x.Status == status).Search(keywords);

            LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Project>> GetAssignedTo(string devName, ProjectStatus? status = null, string keywords = null, OrderModel order = null)
        {
            var query = Projects.Where(proj => proj.ProjectAssignments.Any(x => x.Developer.Nickname == devName)).Search(keywords);

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Project>> GetAssignableTo(string devName, ProjectStatus? status = null, string keywords = null, OrderModel order = null)
        {
            var query = Projects.Where(proj => proj.ProjectAssignments.All(x => x.Developer.Nickname != devName)).Search(keywords);

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return this._context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }
    }
}
