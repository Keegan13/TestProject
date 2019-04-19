using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProjectManagerService
    {
        private readonly DbContext _context;
        private Dictionary<string, ISortScheme<Developer>> _devSorts;
        private Dictionary<string, ISortScheme<Project>> _projSorts;

        public virtual void AddSortScheme<TEntity>(string key, ISortScheme<TEntity> scheme)
        {
            if (typeof(Developer) == typeof(TEntity))
            {
                if (!_devSorts.ContainsKey(key))
                    _devSorts.Add(key, (ISortScheme<Developer>)scheme);
            }
            if (typeof(Project) == typeof(TEntity))
            {

                if (!_projSorts.ContainsKey(key))
                    _projSorts.Add(key, (ISortScheme<Project>)scheme);
            }
        }
        public virtual IEnumerable<string> GetSortSchemes<TEntity>()
        {
            if (typeof(Project) == typeof(TEntity))
                return _projSorts.Keys;
            if (typeof(Developer) == typeof(TEntity))
                return _devSorts.Keys;
            return Enumerable.Empty<string>();
        }

        public ProjectManagerService(ApplicationContext context)
        {
            this._context = context;
            this._devSorts = new Dictionary<string, ISortScheme<Developer>>();
            this._projSorts = new Dictionary<string, ISortScheme<Project>>();
        }
        public void Add<T>(T entity) where T : class => _context.Set<T>().Add(entity);
        public Project GetProject(int id)
        {
            return _context.Set<Project>().Find(id);
        }
        public Project GetProject(string name)
        {
            //?
            return _context.Set<Project>().Where(x => x.Name == name).FirstOrDefault();
        }
        public Developer GetDeveloper(int id)
        {
            return _context.Set<Developer>().Find(id);
        }
        public Task<Developer> GetDeveloper(string nickname)
        {
            return _context.Set<Developer>().Where(x => x.Nickname == nickname).FirstOrDefaultAsync();
        }
        public IEnumerable<Project> GetActiveProjects(int skip = 0, int take = 0)
        {
            var query = _context.Set<Project>().Where(x => x.Status == ProjectStatus.InProgress);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return query.OrderBy(x => x.EndDate).ToArray();
        }
        public async Task<IEnumerable<TEntity>> Get<TEntity>(string sort = null, string keywords = null, bool isAsc = true, int skip = 0, int take = 0) where TEntity:class
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.FullName.Contains(keywords));
            }
            if (string.IsNullOrEmpty(sort)) sort = "fullName";
            if (isAsc) query = query.OrderBy(sort);
            else query = query.OrderByDescending(sort);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return await query.ToArrayAsync();
        }
        public async Task<IEnumerable<Developer>> GetDevelopers(string sort = null, string keywords = null, bool isAsc = true, int skip = 0, int take = 0)
        {
            var query = _context.Set<Developer>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.FullName.Contains(keywords));
            }
            if (string.IsNullOrEmpty(sort)) sort = "fullName";
            if (isAsc) query = query.OrderBy(sort);
            else query = query.OrderByDescending(sort);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return await query.ToArrayAsync();
        }

        public async Task<IEnumerable<Project>> GetProjects(string sortColumn = null, string keywords = null, bool isAsc = true, int skip = 0, int take = 0)
        {
            var query = _context.Set<Project>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                //pre
                query = query.Where(x => x.Name.Contains(keywords));
            }
            if (string.IsNullOrEmpty(sortColumn)) sortColumn = "Name";
            if (isAsc) query = query.OrderBy(sortColumn);
            else query = query.OrderByDescending(sortColumn);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return await query.ToArrayAsync();
        }
        public int CountProjects(string keywords = null)
        {
            var query = _context.Set<Project>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.Name.Contains(keywords));
            }
            return query.Count();
        }
        public int CountDevelopers(string keywords = null)
        {
            var query = _context.Set<Developer>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.FullName.Contains(keywords) | x.Nickname.Contains(keywords));
            }
            return query.Count();
        }

        public void Update<T>(T entity) where T : class => _context.Set<T>().Update(entity);
        public void AssignDeveloper(int projectId, int developerId)
        {

        }
        public void AssignDeveloper(Project project, Developer developer)
        {

        }
        public void DismissDeveloper(Project project, Developer developer)
        {

        }
        public void DismissDeveloper(int projectId, int developerId)
        {

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
        public bool DeveloperExists(Developer developer)
        {
            if (developer.Id > 0) return DeveloperExists(developer.Id);
            if (!string.IsNullOrEmpty(developer.Nickname)) return DeveloperExists(developer.Nickname);
            throw new Exception();
        }
        public bool DeveloperExists(int id)
        {
            if (id <= 0) throw new Exception();
            return _context.Set<Developer>().Any(x => x.Id == id);
        }
        public bool DeveloperExists(string nickname)
        {
            if (string.IsNullOrEmpty(nickname)) throw new ArgumentOutOfRangeException();
            return _context.Set<Developer>().Any(x => x.Nickname == nickname);
        }
        public bool IsAssigned(int projectId, int developerId)
        {
            if (projectId <= 0 || developerId <= 0) throw new ArgumentOutOfRangeException();
            return _context.Set<ProjectDeveloper>().Any(x => x.ProjectId == projectId && x.DeveloperId == developerId);
        }
        public bool IsAssigned(string projectName, string developerNickname)
        {
            if (String.IsNullOrEmpty(projectName) || String.IsNullOrEmpty(developerNickname)) throw new ArgumentOutOfRangeException();
            return _context.Set<ProjectDeveloper>().Any(x => x.Developer.Nickname == developerNickname && x.Project.Name == projectName);
        }
        public bool IsAssigned(Project project, Developer developer)
        {
            if (project.Id > 0 && developer.Id > 0)
                return IsAssigned(project.Id, developer.Id);
            if (!String.IsNullOrEmpty(project.Name) || !String.IsNullOrEmpty(developer.Nickname))
                return IsAssigned(project.Name, developer.Nickname);
            return false;
        }
        public Task<int> SaveChanges() => _context.SaveChangesAsync();

    }
}
