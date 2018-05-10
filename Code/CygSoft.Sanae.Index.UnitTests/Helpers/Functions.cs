using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.UnitTests.Helpers
{
    public static class Functions
    {
        public static string ComparableXml(string sql)
        {
            // Remove white space...
            var output = System.Text.RegularExpressions.Regex.Replace(sql, "[ \t\n\r\v\f]", "");
            return output.ToLower();
        }
    }
}
