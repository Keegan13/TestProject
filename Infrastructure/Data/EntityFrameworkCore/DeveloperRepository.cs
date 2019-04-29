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
    public class DeveloperRepository : IDeveloperRepository
    {
        public DeveloperRepository(ApplicationContext context, ITagRepository tags)
        {
            this._context = context;
            this._tags = tags;
            this.LastQueryTotalCount = 0;
        }

        public virtual int LastQueryTotalCount { get; protected set; }

        protected readonly ApplicationContext _context;

        protected readonly ITagRepository _tags;

        protected DbSet<Developer> Developers
        {
            get => this._context.Set<Developer>();
        }

        protected virtual IQueryable<Developer> DevelopersWithTags
        {
            get => this.Developers.Include(x => x.DeveloperTags).ThenInclude(x => x.Tag);
        }

        protected virtual OrderModel DefaultOrderModel
        {
            get => _defaultOrderModel;
        }

        private static OrderModel _defaultOrderModel = new OrderModel
        {
            SortColumn = nameof(Developer.FullName),
            isAscendingOrder = true
        };

        #region IDeveloperRepository implementation
        //On Create developer has 
        // TAGS
        //  - that not yet exist in Db
        //  - that already exist in Db
        public async Task Add(Developer developer)
        {
            await UpdateTags(developer);

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
            var query = DevelopersWithTags.Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Developer>> GetAssignableTo(string projName, string keywords = null, OrderModel order = null)
        {
            var query = DevelopersWithTags.
                Where(d => d.ProjectAssignments.All(x => x.Project.Name != projName)).Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Developer>> GetAssignedTo(string projName, string keywords = null, OrderModel order = null)
        {
            var query = DevelopersWithTags.Where(d => d.ProjectAssignments.Any(x => x.Project.Name == projName)).Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public Task<Developer> Single(string nickname)
        {
            return DevelopersWithTags.SingleOrDefaultAsync(x => x.Nickname == nickname);
        }


        //On Update developer has 
        // TAGS that:
        //  - should be added to relationship
        //     - not yet exist in Db
        //     - already exist in Db
        //  nope - should be removed from relationship
        public async Task Update(Developer developer)
        {
            await UpdateTags(developer);

            Developers.Update(developer);
        }

        protected virtual async Task UpdateTags(Developer developer)
        {
            //map tags that exist in Db

            var tagsFromDb = await _tags.Get(
                developer.DeveloperTags.
                Select(x => x.Tag.Name));

            //tags that not yet in Db
            List<Tag> newTags = new List<Tag>();
            //    Where(dt => !dbTags.Select(t => t.Name).Contains(dt.Tag.Name)).
            //  Select(x => x.Tag).ToList();

            foreach (var dt in developer.DeveloperTags.ToArray())
            {
                if (tagsFromDb.FirstOrDefault(x => x.Name == dt.Tag.Name) is Tag dbTag)
                {
                    dt.Tag = dbTag;
                }
                else
                {
                    newTags.Add(dt.Tag);
                }

                dt.Developer = developer;
            }

           // await this._tags.AddRange(newTags);
        }

        public Task<int> SaveChangesAsync()
        {
            return this._context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        #endregion
    }
}
