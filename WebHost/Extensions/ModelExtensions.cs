using Host.Models;
using Infrastructure.Entities;
using AutoMapper;
using System.Linq;

namespace Host.Extensions
{
    public static class ModelExtensions
    {
        public static Developer GetInstance(this EditDeveloperViewModel model)
        {
            var instance = Mapper.Map<Developer>(model);

            instance.DeveloperTags = model.Tags.
                Select(x => new DeveloperTag
                {
                    Developer = instance,
                    Tag = new Tag { Name = x }
                }).ToList();

            return instance;
        }

        public static EditDeveloperViewModel GetVM(this Developer developer, string url, string projectUrl = null)
        {
            var model = Mapper.Map<EditDeveloperViewModel>(developer);
            model.Url = url;
            model.Project = projectUrl;
            model.Tags = developer.DeveloperTags.Select(x => x.Tag.Name).ToArray();
            return model;
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
                Skip = filter.Skip ?? 0,
                Take = filter.Take ?? 0
            };
        }

    }
}
