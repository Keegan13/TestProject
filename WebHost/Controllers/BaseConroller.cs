using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseConroller : ControllerBase
    {
        public BaseConroller()
        {

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
