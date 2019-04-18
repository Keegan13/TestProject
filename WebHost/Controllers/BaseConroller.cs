using AutoMapper.Configuration;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Host.Controllers
{
    public class BaseConroller : Controller
    {
        protected readonly ProjectManagerService _mng;
        protected readonly IConfiguration config;
        public BaseConroller(ProjectManagerService projectManager, IConfiguration configuration)
        {
            _mng = projectManager;
            config = configuration;
        }
        public static string Encode(string input)
        {
            return HttpUtility.UrlEncode(input).Replace('+', '-');
        }
        public static string Decode(string encoded)
        {
            return HttpUtility.UrlDecode(encoded.Replace('-', '+'));
        }
    }
}
