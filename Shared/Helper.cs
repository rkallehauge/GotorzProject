using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GotorzProject.Shared
{
    public static class Helper
    {
        public static string ToQueryString(Dictionary<string, string> parameters)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }
            return query.ToString();
        }
    }
}
