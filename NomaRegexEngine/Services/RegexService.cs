using System.Text.RegularExpressions;

namespace NomaRegexEngine.Services
{
    public class RegexService
    {
        public List<string> ApplyRegex(string content, List<string> regexPatterns)
        {
            List<string> allMatches = [];

            foreach (string pattern in regexPatterns)
            {
                MatchCollection matches = Regex.Matches(content, pattern);
                allMatches.AddRange(matches.Select(match => match.Value));
            }

            return allMatches;
        }
    }
}