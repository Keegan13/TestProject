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

        public void Add(Tag tag)
        {
            _context.Add(tag);
        }

        public void AddRange(IEnumerable<Tag> tags)
        {
            this.Tags.AddRange(tags);
        }

        public void Delete(Tag tag)
        {
            var entry = _context.Entry(tag);
            entry.State = EntityState.Deleted;
        }

        public async Task<IEnumerable<Tag>> Get(IEnumerable<string> tagNames)
        {
            return await Tags.Where(x => tagNames.Contains(x.Name)).ToArrayAsync();
        }

        public Task<Tag> Get(string name)
        {
            return Tags.SingleOrDefaultAsync(x => x.Name == name);
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
