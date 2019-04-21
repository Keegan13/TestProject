using Host.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Host.Controllers
{
    public abstract class BaseConroller<T> : Controller
    {
        private Dictionary<string, Func<FilterModel, Task<IEnumerable<T>>>> Sets;
        protected readonly ProjectManagerService _mng;
        public BaseConroller(ProjectManagerService projectManager)
        {
            _mng = projectManager;
            this.Sets = new Dictionary<string, Func<FilterModel, Task<IEnumerable<T>>>>();
            this.Sets.Add("default", DefaultSet);
            InitializeSets(this.Sets);
        }
        protected abstract void InitializeSets(Dictionary<string, Func<FilterModel, Task<IEnumerable<T>>>> sets);
        protected abstract Task<IEnumerable<T>> DefaultSet(FilterModel filter);
        protected Task<IEnumerable<T>> Set(FilterModel filter)
        {
            if (Sets.ContainsKey(filter.Set))
            {
                return Sets[filter.Set].Invoke(filter);
            }
            return Sets["default"].Invoke(filter);
        }




        public static string Encode(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return HttpUtility.UrlEncode(input).Replace('+', '-');
            return null;
        }
        public static string Decode(string encoded)
        {
            if (!string.IsNullOrEmpty(encoded))
                return HttpUtility.UrlDecode(encoded.Replace('-', '+'));
            return null;
        }
    }
}
