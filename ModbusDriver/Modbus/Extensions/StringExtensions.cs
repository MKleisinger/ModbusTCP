using System.Text.RegularExpressions;

namespace Modbus.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all white space from a string value.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveAllWhiteSpace(this string s)
        {
            return Regex.Replace(s, @"\s+", "");
        }
    }
}
