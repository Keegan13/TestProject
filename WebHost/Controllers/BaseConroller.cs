using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Host.Controllers
{
    public class BaseConroller : Controller
    {
        protected readonly ProjectManagerService _mng;
        public BaseConroller(ProjectManagerService projectManager)
        {
            _mng = projectManager;
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
