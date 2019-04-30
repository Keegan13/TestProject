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
            this._tagRepo = tags;
            this.LastQueryTotalCount = 0;
        }

        public virtual int LastQueryTotalCount { get; protected set; }

        protected readonly ApplicationContext _context;

        //only 1 reference (sign of low cohesion)
        protected readonly ITagRepository _tagRepo;

        protected DbSet<Developer> Developers
        {
            get => this._context.Set<Developer>();
        }

        protected virtual IQueryable<Developer> DevelopersIncludeTags
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
        public async Task Add(Developer developer)//command
        {
            await UpdateTags(developer);

            Developers.Add(developer);
        }

        public void Delete(Developer developer)//command
        {
            Developers.Remove(developer);
        }

        public Task<bool> Exist(string devNickname)//query
        {
            return Developers.AnyAsync(x => x.Nickname == devNickname);
        }

        public Task<bool> Exist(int id)//query
        {
            return Developers.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Developer>> Get(string keywords = null, OrderModel order = null)
        {
            var query = DevelopersIncludeTags.Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Developer>> GetAssignableTo(string projName, string keywords = null, OrderModel order = null)
        {
            var query = DevelopersIncludeTags
                .Where(d => d.ProjectAssignments.All(pa => pa.Project.Name != projName))
                .Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<IEnumerable<Developer>> GetAssignedTo(string projName, string keywords = null, OrderModel order = null)
        {
            var query = DevelopersIncludeTags
                .Where(d => d.ProjectAssignments.Any(x => x.Project.Name == projName))
                .Search(keywords);

            this.LastQueryTotalCount = await query.CountAsync();

            if (order == null)
            {
                order = DefaultOrderModel;
            }

            return await query.ApplyOrderModel(order).ToArrayAsync();
        }

        public async Task<Developer> Single(string nickname)
        {
            var developer = await DevelopersIncludeTags
                .SingleOrDefaultAsync(x => x.Nickname == nickname);

            this.LastQueryTotalCount = developer == null ? 0 : 1;

            return developer;
        }

        //On Update developer has 
        // TAGS that:
        //  - should be added to relationship
        //     - not yet exist in Db
        //     - already exist in Db
        //  nope - should be removed from relationship
        public async Task Update(Developer developer)//command
        {
            await UpdateTags(developer);

            Developers.Update(developer);
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

        protected virtual async Task UpdateTags(Developer developer)
        {
            // load Tags that is assigned to developer instance and exist in Db
            var existingTags = await _tagRepo.Get(
                developer.DeveloperTags
                .Select(x => x.Tag.Name)
                );

            foreach (var dt in developer.DeveloperTags)
            {
                if (existingTags.FirstOrDefault(x => x.Name == dt.Tag.Name) is Tag existingTag)
                {
                    dt.Tag = existingTag;// replacing all existing tags with tags loaded from Db (which is tracked by DbContext)
                }

                dt.Developer = developer;
            }
        }
    }
}
