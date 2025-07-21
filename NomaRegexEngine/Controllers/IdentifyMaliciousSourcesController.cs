using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using NomaRegexEngine.Models;
using NomaRegexEngine.Services;
using NomaRegexEngine.Utils;

namespace NomaRegexEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentifyMaliciousSourcesController(
        RegexService regexService,
        HttpClient httpClient) : ControllerBase
    {
        private readonly RegexService _regexService = regexService;
        private readonly HttpClient _httpClient = httpClient;

        [HttpPost("from-website")]
        public async Task<IActionResult> IdentifyFromWebsite([FromBody] SourceRequest request)
        {
            if (request.SourceType != SourceType.Http)
            {
                return BadRequest("Website source must be of type 'Http'.");
            }

            string html = await _httpClient.GetStringAsync(request.Source);
            List<string> matches = _regexService.ApplyRegex(html, request.RegexPatterns);

            return Ok(matches);
        }

        [HttpPost("from-code")]
        public async Task<IActionResult> IdentifyFromCode([FromBody] SourceRequest request)
        {
            string code = request.SourceType switch
            {
                SourceType.Http => await _httpClient.GetStringAsync(request.Source),
                SourceType.FileSystem => await System.IO.File.ReadAllTextAsync(request.Source),
                _ => throw new ArgumentException("Unknown SourceType")
            };

            List<string> matches = _regexService.ApplyRegex(code, request.RegexPatterns);

            return Ok(matches);
        }

        [HttpPost("from-notebook")]
        public async Task<IActionResult> IdentifyFromNotebook([FromBody] SourceRequest request)
        {
            string json = request.SourceType switch
            {
                SourceType.Http => await _httpClient.GetStringAsync(request.Source),
                SourceType.FileSystem => await System.IO.File.ReadAllTextAsync(request.Source),
                _ => throw new ArgumentException("Unknown SourceType")
            };

            List<string> sourceCellsText = NotebookParser.ExtractSourceCellsText(json);
            ConcurrentBag<string> matches = [];

            Parallel.ForEach(sourceCellsText, sourceCell =>
            {
                List<string> result = _regexService.ApplyRegex(sourceCell, request.RegexPatterns);
                
                foreach (string match in result)
                {
                    matches.Add(match);
                }
            });

            return Ok(matches);
        }
    }
}
