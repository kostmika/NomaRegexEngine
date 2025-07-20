using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NomaRegexEngine.Services
{
    public class RegexService
    {
        public List<string> ApplyRegex(string content, string pattern)
        {
            var results = new List<string>();
            var matches = Regex.Matches(content, pattern);

            foreach (Match match in matches)
                results.Add(match.Value);

            return results;
        }
    }
}
