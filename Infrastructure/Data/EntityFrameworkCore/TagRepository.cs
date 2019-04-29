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
    public class TagRepository : ITagRepository
    {
        protected readonly ApplicationContext _context;

        protected DbSet<Tag> Tags => this._context.Set<Tag>();

        public TagRepository(ApplicationContext context)
        {
            this._context = context;
        }

        public void AddOrLoad(Tag tag)
        {

            //  
            //
            //
            //
            //
        }

        public Task AddOrLoadRange(IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<Tag> tags)
        {
            return this.Tags.AddRangeAsync(tags);
        }

        public void Delete(Tag tag)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tag>> Get(IEnumerable<string> tagNames)
        {
            return await Tags.Where(x => tagNames.Contains(x.Name)).ToArrayAsync();
        }

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return this._context.SaveChangesAsync();
        }
    }
}
