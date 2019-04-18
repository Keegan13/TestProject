using Host.Models;
using Infrastructure.Entities;
using AutoMapper;

namespace Host.Extensions
{
    public static class ModelExtensions
    {
        public static Developer GetInstance(this EditDeveloperViewModel model)
        {
            return Mapper.Map<Developer>(model);
        }
        public static EditDeveloperViewModel GetVM(this Developer developer, string url)
        {
            var result = Mapper.Map<EditDeveloperViewModel>(developer);
            result.Url = url;
            return result;
        }

        public static Project GetInstance(this EditProjectViewModel model)
        {
            return Mapper.Map<Project>(model);
        }
        public static EditProjectViewModel GetVM(this Project project, string url)
        {
            var result = Mapper.Map<EditProjectViewModel>(project);
            result.Url = url;
            return result;
        }
    }
}
