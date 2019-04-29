using Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface IDeveloperRepository : IUnitOfWork, IPaginationRepository
    {
        Task<Developer> Single(string nickname);

        void Delete(Developer developer);

        Task Add(Developer developer);

        Task Update(Developer developer);

        Task<bool> Exist(string devNickname);

        Task<bool> Exist(int id);

        Task<IEnumerable<Developer>> Get(string keywords = null, OrderModel order = null);

        Task<IEnumerable<Developer>> GetAssignedTo(string projName, string keywords = null, OrderModel order = null);

        Task<IEnumerable<Developer>> GetAssignableTo(string projName, string keywords = null, OrderModel order = null);
    }
}
