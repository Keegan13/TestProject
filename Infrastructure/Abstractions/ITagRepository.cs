using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface ITagRepository : IUnitOfWork
    {
        Task<IEnumerable<Tag>> Get(IEnumerable<string> names);

        void AddOrLoad(Tag tag);
        Task AddRange(IEnumerable<Tag> tags);
        Task AddOrLoadRange(IEnumerable<Tag> tags);
        void Delete(Tag tag);
    }
}
