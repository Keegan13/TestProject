using Infrastructure.Abstractions;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    [Obsolete]
    public class ProjectManagerService : IProjectRepository, IDeveloperRepository, IPaginationRepository, IProjectAssignments//,IDisposable 
    {
        private readonly IProjectRepository Repo;
        protected readonly DbContext _context;

        public int LastQueryTotalCount { get; protected set; }




        public ProjectManagerService(IProjectRepository Repo, ApplicationContext context)
        {
            this.Repo = Repo;
            this._context = context;
            this.LastQueryTotalCount = 0;
        }




        #region READ



        public Task<Project> GetProject(string name)
        {
            return _context.Set<Project>().SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Project>> GetProjectsAssignedToDeveloper(string devName, OrderModel model = null)
        {
            var query = _context.Set<Project>().Where(proj => proj.ProjectAssignments.Any(x => x.Developer.Nickname == devName));


            LastQueryTotalCount = await query.CountAsync();

            if (model == null)
            {
                model = new OrderModel { SortColumn = "Name", isAscendingOrder = true };
            }

            return await query.ApplyOrderModel(model).ToArrayAsync();


            //return _context.Set<Developer>().Include(x => x.ProjectAssignments).FirstOrDefault(x => x.Nickname == devNickname).ProjectAssignments.Select(x => x.Project);

            //if (await Single(devNickname) is Developer dev)
            //{
            //    int devId = dev.Id;
            //    IEnumerable<int> projIds = this._context.Set<ProjectDeveloper>().Where(x => x.DeveloperId == devId).Select(x => x.ProjectId).ToArray();

            //    var query = _context.Set<Project>().Where(x => projIds.Contains(x.Id));
            //    this.LastQueryTotalCount = await query.CountAsync();
            //    query = query.ApplyOrderModel(model);
            //    return await query.ToArrayAsync();
            //}
            //return Enumerable.Empty<Project>();
        }

        public async Task<IEnumerable<Developer>> GetAssignedDevelopers(string projName, string keywords = null, OrderModel model = null)
        {
            if (await GetProject(projName) is Project proj)
            {
                int projId = proj.Id;
                IEnumerable<int> devIds = this._context.Set<ProjectAssignment>().Where(x => x.ProjectId == projId).Select(x => x.DeveloperId).ToArray();
                var query = _context.Set<Developer>().Where(x => devIds.Contains(x.Id));
                if (!string.IsNullOrEmpty(keywords))
                {
                    query = query.Search(keywords);
                }
                this.LastQueryTotalCount = await query.CountAsync();

                query = query.ApplyOrderModel(model);
                return await query.ToArrayAsync();
            }
            return Enumerable.Empty<Developer>();
        }
        public async Task<IEnumerable<Developer>> GetNotAssignedDevelopers(string projName, string keywords = null, OrderModel model = null)
        {
            if (await GetProject(projName) is Project proj)
            {
                int projId = proj.Id;
                IEnumerable<int> devIds = this._context.Set<ProjectAssignment>().Where(x => x.ProjectId == projId).Select(x => x.DeveloperId).ToArray();
                var query = _context.Set<Developer>().Where(x => !devIds.Contains(x.Id));

                if (!string.IsNullOrEmpty(keywords))
                    query = query.Search(keywords);
                this.LastQueryTotalCount = await query.CountAsync();
                query = query.ApplyOrderModel(model);
                return await query.ToArrayAsync();
            }
            return Enumerable.Empty<Developer>();
        }

        public async Task<IEnumerable<Project>> GetNotAssignedProjects(string nickname, string keywords = null, OrderModel model = null)
        {
            if (await Single(nickname) is Developer dev)
            {
                int devId = dev.Id;
                IEnumerable<int> projIds = this._context.Set<ProjectAssignment>().Where(x => x.DeveloperId == devId).Select(x => x.ProjectId).ToArray();

                var query = _context.Set<Project>().Where(x => !projIds.Contains(x.Id));

                if (!string.IsNullOrEmpty(keywords))
                {
                    query = query.Search(keywords);
                }
                this.LastQueryTotalCount = await query.CountAsync();
                query = query.ApplyOrderModel(model);
                return await query.ToArrayAsync();
            }
            return Enumerable.Empty<Project>();
        }

        //OK
        public async Task<IEnumerable<Project>> GetByStatus(ProjectStatus status, string keywords = null, OrderModel model = null)
        {
            var query = _context.Set<Project>().Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Search(keywords);
            }

            this.LastQueryTotalCount = await query.CountAsync();

            if (model == null)
            {
                model = new OrderModel { SortColumn = nameof(Project.EndDate), isAscendingOrder = true };
                //  Completed EndDate DSC // recently completed
                //  InProgress EndDate ASC // closest to deadline
                //  UnStarted StartDate ASC // almost there
                if (status == ProjectStatus.Completed) model.isAscendingOrder = false;
                if (status == ProjectStatus.UnStarted) model.SortColumn = nameof(Project.StartDate);
            }
            return await query.ApplyOrderModel(model).ToArrayAsync();
        }

        //OK
        public Task<Developer> Single(string nickname)
        {
            return _context.Set<Developer>().SingleOrDefaultAsync(x => x.Nickname == nickname);
        }
        //OK
        public async Task<IEnumerable<TEntity>> Get<TEntity>(string keywords = null, OrderModel model = null) where TEntity : class
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Search(keywords);
            }
            this.LastQueryTotalCount = await query.CountAsync();
            query = query.ApplyOrderModel(model);
            return await query.ToArrayAsync();
        }


        #endregion

        #region  Create
        public void Add<TEntity>(TEntity entity) where TEntity : class => _context.Set<TEntity>().Add(entity);
        #endregion
        #region Delete
        public void Delete<TEntilty>(TEntilty entity) where TEntilty : class
        {
            _context.Set<TEntilty>().Remove(entity);
        }
        #endregion
        #region Update

        public void Update<TEntity>(TEntity entity) where TEntity : class => _context.Set<TEntity>().Update(entity);

        #endregion

        #region Unitily


        public int CountProjects(string keywords = null)
        {
            var query = _context.Set<Project>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.Name.Contains(keywords));
            }
            return query.Count();
        }




        public async Task Assign(Project project, Developer developer)
        {
            await _context.Set<ProjectAssignment>().AddAsync(new ProjectAssignment() { Project = project, Developer = developer });
        }
        public async Task Unassign(Project project, Developer developer)
        {
            if (await _context.Set<ProjectAssignment>().SingleAsync(x => x.DeveloperId == developer.Id && x.ProjectId == project.Id) is ProjectAssignment bind)
                _context.Set<ProjectAssignment>().Remove(bind);
        }
        public async Task<bool> ProjectExists(Project project)
        {
            if (project.Id > 0) return await ProjectExists(project.Id);
            if (!string.IsNullOrEmpty(project.Name)) return await ProjectExists(project.Name);
            throw new Exception();
        }
        public Task<bool> ProjectExists(int id)
        {
            if (id <= 0) throw new Exception();
            return _context.Set<Project>().AnyAsync(x => x.Id == id);
        }
        public Task<bool> ProjectExists(string name)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentOutOfRangeException();
            return _context.Set<Project>().AnyAsync(x => x.Name == name);
        }
        public async Task<bool> DeveloperExists(Developer developer)
        {
            if (developer.Id > 0) return DeveloperExists(developer.Id);
            if (!string.IsNullOrEmpty(developer.Nickname)) return await DeveloperExists(developer.Nickname);
            throw new Exception();
        }
        public bool DeveloperExists(int id)
        {
            if (id <= 0) throw new Exception();
            return _context.Set<Developer>().Any(x => x.Id == id);
        }
        public Task<bool> DeveloperExists(string nickname)
        {
            if (string.IsNullOrEmpty(nickname)) throw new ArgumentOutOfRangeException();
            return _context.Set<Developer>().AnyAsync(x => x.Nickname == nickname);
        }
        public Task<bool> IsAssigned(int projectId, int developerId)
        {
            if (projectId <= 0 || developerId <= 0) throw new ArgumentOutOfRangeException();
            return _context.Set<ProjectAssignment>().AnyAsync(x => x.ProjectId == projectId && x.DeveloperId == developerId);
        }
        public Task<bool> IsAssigned(string projectName, string developerNickname)
        {
            if (String.IsNullOrEmpty(projectName) || String.IsNullOrEmpty(developerNickname)) throw new ArgumentOutOfRangeException();
            return _context.Set<ProjectAssignment>().AnyAsync(x => x.Developer.Nickname == developerNickname && x.Project.Name == projectName);
        }

        public async Task<bool> IsAssigned(Project project, Developer developer)
        {
            if (project.Id > 0 && developer.Id > 0)
                return await IsAssigned(project.Id, developer.Id);
            if (!String.IsNullOrEmpty(project.Name) || !String.IsNullOrEmpty(developer.Nickname))
                return await IsAssigned(project.Name, developer.Nickname);
            return false;
        }






        #region IProjectRepotsitory implementations
        Task<Project> IProjectRepository.Single(string name)
        {
            throw new NotImplementedException();
        }

        void IProjectRepository.Delete(Project project)
        {
            throw new NotImplementedException();
        }

        void IProjectRepository.Add(Project project) => this.Add(project);


        Task<IEnumerable<Project>> IProjectRepository.Get(string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Project>> IProjectRepository.GetByStatus(ProjectStatus status, string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Project>> IProjectRepository.GetAssignedTo(string devName, ProjectStatus? status, string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Project>> IProjectRepository.GetAssignableTo(string devName, ProjectStatus? status, string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region IDeveloperRepository implementation
        Task<Developer> IDeveloperRepository.Single(string nickname)
        {
            throw new NotImplementedException();
        }

        void IDeveloperRepository.Delete(Developer developer)
        {
            throw new NotImplementedException();
        }

        void IDeveloperRepository.Add(Developer developer)
        {
            throw new NotImplementedException();
        }

        void IDeveloperRepository.Update(Developer developer)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Developer>> IDeveloperRepository.Get(string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Developer>> IDeveloperRepository.GetAssignedTo(string projName, string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Developer>> IDeveloperRepository.GetAssignableTo(string projName, string keywords, OrderModel order)
        {
            throw new NotImplementedException();
        }

        public Task Assign(string projName, string devNickname)
        {
            throw new NotImplementedException();
        }

        public Task Unassign(string projName, string devNickname)
        {
            throw new NotImplementedException();
        }



        #endregion

        #endregion


        #region IUnitOfWork implementation
        public Task<int> SaveChanges() => _context.SaveChangesAsync();

        Task<bool> IProjectRepository.Exist(string projName)
        {
            throw new NotImplementedException();
        }

        Task<bool> IProjectRepository.Exist(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDeveloperRepository.Exist(string devNickname)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDeveloperRepository.Exist(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        int IUnitOfWork.SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Project project)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
