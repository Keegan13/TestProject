using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface IProjectRepository : IUnitOfWork, IPaginationRepository
    {
        Task<Project> Single(string name);

        void Delete(Project project);

        void Add(Project project);

        Task<bool> Exist(string projName);

        Task<bool> Exist(int id);

        Task<IEnumerable<Project>> Get(string keywords = null, OrderModel order = null);

        Task<IEnumerable<Project>> GetByStatus(ProjectStatus status, string keywords = null, OrderModel order = null);

        Task<IEnumerable<Project>> GetAssignedTo(string devName, Nullable<ProjectStatus> status = null, string keywords = null, OrderModel order = null);

        Task<IEnumerable<Project>> GetAssignableTo(string devName, Nullable<ProjectStatus> status = null, string keywords = null, OrderModel order = null);
    }


}
