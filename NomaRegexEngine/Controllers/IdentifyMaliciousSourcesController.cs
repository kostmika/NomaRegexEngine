using Microsoft.AspNetCore.Mvc;
using NomaRegexEngine.Models;
using NomaRegexEngine.Services;
using NomaRegexEngine.Utils;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            string html = await _httpClient.GetStringAsync(request.Source);
            List<string> matches = _regexService.ApplyRegex(html, request.RegexPattern);

            return Ok(matches);
        }

        [HttpPost("from-code")]
        public async Task<IActionResult> IdentifyFromCode([FromBody] SourceRequest request)
        {
            string code;

            if (request.Source.StartsWith("http"))
            {
                code = await _httpClient.GetStringAsync(request.Source);
            }
            else
            {
                code = System.IO.File.ReadAllText(request.Source);
            }

            List<string> matches = _regexService.ApplyRegex(code, request.RegexPattern);

            return Ok(matches);
        }

        [HttpPost("from-notebook")]
        public async Task<IActionResult> IdentifyFromNotebook([FromBody] SourceRequest request)
        {
            string json;

            if (request.Source.StartsWith("http"))
            {
                json = await _httpClient.GetStringAsync(request.Source);
            }
            else
            {
                json = System.IO.File.ReadAllText(request.Source);
            }

            List<string> sourceCellsText = NotebookParser.ExtractSourceCellsText(json);
            List<string> matches = [];

            foreach (string sourceCell in sourceCellsText)
            {
                matches = (List<string>)matches.Concat(_regexService.ApplyRegex(sourceCell, request.RegexPattern));
            }

            return Ok(matches);
        }
    }
}
