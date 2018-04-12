using System.Text.RegularExpressions;

namespace Gnsso.Html
{
    public static class RegexUtils
    {
        public static bool TryMatch(string input, string pattern, out Match match)
        {
            try
            {
                return (match = Regex.Match(input, pattern)).Success;
            }
            catch
            {
                match = null;
                return false;
            }
        }
    }
}
