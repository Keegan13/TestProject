using Host.Models;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Host.Extensions
{
    public static class ModelExtensions
    {
        public static Developer GetInstance(this EditDeveloperViewModel model)
        {
            return Mapper.Map<Developer>(model);
        }
        public static EditDeveloperViewModel GetVM(this Developer developer)
        {
            return Mapper.Map<EditDeveloperViewModel>(developer);
        }
    }
}
