using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace NomaRegexEngine.Utils
{
    public static class NotebookParser
    {
        public static List<string> ExtractSourceCellsText(Stream jsonStream)
        {
            List<string> results = [];
            JsonDocument doc = JsonDocument.Parse(jsonStream);

            if (doc.RootElement.TryGetProperty("cells", out JsonElement cells))
            {
                foreach (JsonElement cell in cells.EnumerateArray())
                {

                    if (cell.TryGetProperty("source", out JsonElement source))
                    {
                        results.Add(source.GetString() ?? "");
                    }
                }
            }

            return results;
        }
    }
}
