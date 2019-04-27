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

        public static EditDeveloperViewModel GetVM(this Developer developer, string url, string projectUrl = null)
        {
            var result = Mapper.Map<EditDeveloperViewModel>(developer);
            result.Url = url;
            result.Project = projectUrl;
            return result;
        }

        public static Project GetInstance(this EditProjectViewModel model)
        {
            return Mapper.Map<Project>(model);
        }
        public static EditProjectViewModel GetVM(this Project project, string url, string developerUrl = null)
        {
            var result = Mapper.Map<EditProjectViewModel>(project);
            result.Url = url;
            result.Developer = developerUrl;
            return result;
        }

        public static OrderModel GetOrderModel(this FilterModel filter)
        {
            if (filter == null) return null;
            return new OrderModel
            {
                SortColumn = filter.Sort,
                isAscendingOrder = filter.Order.HasValue ? filter.Order.Value == OrderDirection.Ascending : true,
                Skip = filter.Skip.HasValue ? filter.Skip.Value : 0,
                Take = filter.Take.HasValue ? filter.Take.Value : 0
            };
        }

    }
}
