using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Interfaces
{
    public static class StringExtensions
    {
        public static string Replaces(this string str, Dictionary<string, string> replacements)
        {
            StringBuilder newStr = new StringBuilder(str);
            return replacements.Aggregate(newStr, (current, replacement) => current.Replace(replacement.Key, replacement.Value)).ToString();
        }
    }
}
