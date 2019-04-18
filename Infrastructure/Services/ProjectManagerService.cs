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
        public ProjectManagerService(ApplicationContext context)
        {
            this._context = context;
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
        public Developer GetDeveloper(string nickname)
        {
            return _context.Set<Developer>().Where(x => x.Nickname == nickname).FirstOrDefault();
        }
        public IEnumerable<Project> GetActiveProjects(int skip = 0, int take = 0)
        {
            var query = _context.Set<Project>().Where(x => x.Status == ProjectStatus.InProgress);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return query.OrderBy(x => x.EndDate).ToArray();
        }
        public IEnumerable<Developer> GetDevelopers(int skip = 0, int take = 0)
        {
            var query = _context.Set<Developer>().AsQueryable();
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return query.OrderBy(x => x.Nickname).ToArray();
        }
        public IEnumerable<Project> GetProjects(int skip = 0, int take = 0)
        {
            var query = _context.Set<Project>().AsQueryable();
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return query.OrderByDescending(x => x.EndDate).ToArray();
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
        public bool ProjectExists(Project project)
        {
            if (ProjectExists(project.Id)) return true;
            return ProjectExists(project.Name);
        }
        public bool ProjectExists(int id)
        {
            if (id <= 0) throw new Exception();
            return _context.Set<Project>().Any(x => x.Id == id);
        }
        public bool ProjectExists(string name)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentOutOfRangeException();
            return _context.Set<Project>().Any(x => x.Name == name);
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
