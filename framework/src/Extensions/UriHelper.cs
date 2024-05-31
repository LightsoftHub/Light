using Light.Extensions;
using System.Linq;
using System.Web;

namespace Light.Extensions
{
    public static class UriHelper
    {
        /// <summary>
        /// Build URL query for get data from an object
        /// </summary>
        public static string BuildQuery(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}
