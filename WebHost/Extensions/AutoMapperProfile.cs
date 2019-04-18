using AutoMapper;
using Infrastructure.Entities;
using Host.Models;

namespace Host.Extensions
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Developer,EditDeveloperViewModel>().ReverseMap();
            CreateMap<Project, EditProjectViewModel>().ReverseMap();
        }
    }
}
