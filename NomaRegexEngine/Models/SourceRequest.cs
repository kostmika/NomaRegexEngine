using System.ComponentModel.DataAnnotations;

namespace NomaRegexEngine.Models
{
    public class SourceRequest
    {
        [Required]
        public string Source { get; set; } = string.Empty;

        [Required]
        public string RegexPattern { get; set; } = string.Empty;
    }
}
