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

        public static void Update(this EditDeveloperViewModel model, Developer toUpdate)
        {
            toUpdate.FullName = model.FullName;
            toUpdate.Nickname = model.Nickname;
        }

        public static void Update(this EditProjectViewModel model, Project toUpdate)
        {
            toUpdate.Name = model.Name;
            toUpdate.StartDate = model.StartDate.HasValue ? model.StartDate.Value : toUpdate.StartDate;
            toUpdate.EndDate = model.EndDate.HasValue ? model.EndDate.Value : toUpdate.EndDate;
            toUpdate.Status = model.Status.HasValue ? model.Status.Value : toUpdate.Status;
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
