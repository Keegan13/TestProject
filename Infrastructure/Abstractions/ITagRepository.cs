using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface ITagRepository : IUnitOfWork
    {
        void Add(Tag tag);
        Task<IEnumerable<Tag>> Get(IEnumerable<string> names);
        Task<Tag> Get(string name);
        void AddRange(IEnumerable<Tag> tags);
        void Delete(Tag tag);
    }
}
