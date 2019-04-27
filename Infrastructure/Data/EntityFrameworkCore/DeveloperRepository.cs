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
    public class DeveloperRepository : IDeveloperRepository, IPaginationRepository
    {
        protected readonly ApplicationContext _context;
        public int LastQueryTotalCount { get; set; }
        protected DbSet<Developer> Developers => this._context.Set<Developer>();

        protected virtual OrderModel DefaultOrderModel => new OrderModel { SortColumn = nameof(Developer.FullName), isAscendingOrder = true };

        public DeveloperRepository(ApplicationContext context)
        {
            this._context = context;
            this.LastQueryTotalCount = 0;
        }


        public void Add(Developer developer)
        {
            Developers.Add(developer);
        }

        public void Delete(Developer developer)
        {
            Developers.Remove(developer);
        }

        public Task<bool> Exist(string devNickname)
        {
            return Developers.AnyAsync(x => x.Nickname == devNickname);
        }

        public Task<bool> Exist(int id)
        {
            return Developers.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Developer>> Get(string keywords = null, OrderModel order = null)
        {
            var query = Developers.Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Developer>> GetAssignableTo(string projName, string keywords = null, OrderModel order = null)
        {
            var query = Developers.Where(d => d.ProjectAssignments.All(x => x.Project.Name != projName)).Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Developer>> GetAssignedTo(string projName, string keywords = null, OrderModel order = null)
        {
            var query = Developers.Where(d => d.ProjectAssignments.Any(x => x.Project.Name == projName)).Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public Task<Developer> Single(string nickname)
        {
            return Developers.SingleOrDefaultAsync(x => x.Nickname == nickname);
        }

        public void Update(Developer developer)
        {
            Developers.Update(developer);
        }

        public Task<int> SaveChangesAsync() => this._context.SaveChangesAsync();

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
