using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NomaRegexEngine.Models
{
    public enum SourceType
    {
        Http,
        FileSystem
    }

    public class SourceRequest
    {
        [Required]
        public string Source { get; set; } = string.Empty;

        [Required]
        public List<string> RegexPatterns { get; set; } = [];

        public SourceType SourceType { get; set; }
    }
}
