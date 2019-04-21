﻿using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public void Add<TEntity>(TEntity entity) where TEntity : class => _context.Set<TEntity>().Add(entity);
        public IQueryable<TEntity> Set<TEntity>() where TEntity : class => _context.Set<TEntity>();
        //public Project GetProject(int id)
        //{
        //    return _context.Set<Project>().Find(id);
        //}

        public int LastQueryTotalCount = 0;

        public Task<Project> GetProject(string name)
        {
            return _context.Set<Project>().SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Project>> GetAssignedProjects(string devNickname)
        {
            if (await GetDeveloper(devNickname) is Developer dev)
            {
                int devId = dev.Id;
                IEnumerable<int> projIds = this._context.Set<ProjectDeveloper>().Where(x => x.DeveloperId == devId).Select(x => x.ProjectId).ToArray();
                return await _context.Set<Project>().Where(x => projIds.Contains(x.Id)).ToArrayAsync();
            }
            return Enumerable.Empty<Project>();
        }

        public async Task<IEnumerable<Developer>> GetAssignedDevelopers(string projName)
        {
            if (await GetProject(projName) is Project proj)
            {
                int projId = proj.Id;
                IEnumerable<int> devIds = this._context.Set<ProjectDeveloper>().Where(x => x.ProjectId == projId).Select(x => x.DeveloperId).ToArray();
                return await _context.Set<Developer>().Where(x => devIds.Contains(x.Id)).ToArrayAsync();
            }
            return Enumerable.Empty<Developer>();
        }


        private async Task<IEnumerable<Project>> GetProjectByStatus(ProjectStatus status, string sortColumn = null, bool isAsc = true, int skip = 0, int take = 0)
        {
            var query = _context.Set<Project>().Where(x => x.Status == status);
            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = isAsc ? query.OrderBy(sortColumn) : query.OrderByDescending(sortColumn);
            }
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return await query.ToArrayAsync();
        }

        public Task<IEnumerable<Project>> GetCompletedProjects(string sortColumn = null, bool isAsc = true, int skip = 0, int take = 0)
        {
            return GetProjectByStatus(ProjectStatus.Completed, sortColumn, isAsc, skip, take);
        }

        public Task<IEnumerable<Project>> GetUpcommingProjects(string sortColumn = null, bool isAsc = true, int skip = 0, int take = 0)
        {
            return GetProjectByStatus(ProjectStatus.UnStarted, sortColumn, isAsc, skip, take);
        }
        public Task<IEnumerable<Project>> GetActiveProjects(string sortColumn = null, bool isAsc = true, int skip = 0, int take = 0)
        {
            return GetProjectByStatus(ProjectStatus.InProgress, sortColumn, isAsc, skip, take);
        }

        //public Developer GetDeveloper(int id)
        //{
        //    return _context.Set<Developer>().Find(id);
        //}
        public Task<Developer> GetDeveloper(string nickname)
        {
            return _context.Set<Developer>().SingleOrDefaultAsync(x => x.Nickname == nickname);
        }
        public IEnumerable<Project> GetActiveProjects(int skip = 0, int take = 0)
        {
            var query = _context.Set<Project>().Where(x => x.Status == ProjectStatus.InProgress);
            if (skip > 0) query = query.Skip(skip);
            if (take > 0) query = query.Take(take);
            return query.OrderBy(x => x.EndDate).ToArray();
        }
        public async Task<IEnumerable<TEntity>> Get<TEntity>(string sortColumn = null, string keywords = null, bool isAsc = true, int skip = 0, int take = 0) where TEntity : class
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Search(keywords);
            }
            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = isAsc ? query.OrderBy(sortColumn) : query.OrderByDescending(sortColumn);
            }
            this.LastQueryTotalCount = await query.CountAsync();
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

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
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
        public async Task Assign(Project project, Developer developer)
        {
            await _context.Set<ProjectDeveloper>().AddAsync(new ProjectDeveloper() { Project = project, Developer = developer });
        }
        public async Task Unassign(Project project, Developer developer)
        {
            if (await _context.Set<ProjectDeveloper>().SingleAsync(x => x.DeveloperId == developer.Id && x.ProjectId == project.Id) is ProjectDeveloper bind)
                _context.Set<ProjectDeveloper>().Remove(bind);
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
            return _context.Set<ProjectDeveloper>().AnyAsync(x => x.ProjectId == projectId && x.DeveloperId == developerId);
        }
        public Task<bool> IsAssigned(string projectName, string developerNickname)
        {
            if (String.IsNullOrEmpty(projectName) || String.IsNullOrEmpty(developerNickname)) throw new ArgumentOutOfRangeException();
            return _context.Set<ProjectDeveloper>().AnyAsync(x => x.Developer.Nickname == developerNickname && x.Project.Name == projectName);
        }
        public async Task<bool[]> IsAssigned(Project project, IEnumerable<Developer> developers)
        {
            //int projId = project.Id;
            //int[] devsId = developers.Select(x => x.Id);
            //var ids = _context.Set<ProjectDeveloper>().Where(x => x.ProjectId == projId).Select(x => x.DeveloperId).ToArrayAsync();
            //var result = new bool[developers.Count()];


            return null;
        }

        public async Task<bool> IsAssigned(Project project, Developer developer)
        {
            if (project.Id > 0 && developer.Id > 0)
                return await IsAssigned(project.Id, developer.Id);
            if (!String.IsNullOrEmpty(project.Name) || !String.IsNullOrEmpty(developer.Nickname))
                return await IsAssigned(project.Name, developer.Nickname);
            return false;
        }
        public Task<int> SaveChanges() => _context.SaveChangesAsync();

    }
}
