using Infrastructure.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface IProjectAssignments: IUnitOfWork
    {
        //Task Assign(string projName, string devNickname);
        Task Assign(Project project, Developer developer);
        Task Unassign(Project project, Developer developer);
        Task Unassign(string projName, string devNickname);
        Task<bool> IsAssigned(string projName, string devNickname);
        //Task<bool> IsAssigned(Project project, Developer developer);
    }
}
