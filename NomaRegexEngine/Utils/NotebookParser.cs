using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace NomaRegexEngine.Utils
{
    public static class NotebookParser
    {
        public static List<string> ExtractSourceCellsText(string json)
        {
            List<string> results = [];
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement cells;

            if (doc.RootElement.TryGetProperty("cells", out cells))
            {
                foreach (JsonElement cell in cells.EnumerateArray())
                {
                    JsonElement source;

                    if (cell.TryGetProperty("source", out source))
                    {
                        List<string> lines = [];

                        foreach (JsonElement line in source.EnumerateArray())
                        {
                            lines.Add(line.GetString() ?? "");
                        }

                        results.Add(string.Join("", lines));
                    }
                }
            }

            return results;
        }

        //    StringBuilder sourceBuilder = new StringBuilder();
        //    JsonNode? doc = JsonNode.Parse(json);
        //    JsonArray? cells = doc?["cells"]?.AsArray();

        //    if (cells == null)
        //    {
        //        return string.Empty;
        //    }

        //    foreach (JsonNode? cell in cells)
        //    {
        //        JsonArray? sourceLines = cell?["source"]?.AsArray();

        //        if (sourceLines != null)
        //        {
        //            foreach (JsonNode? line in sourceLines)
        //            {
        //                sourceBuilder.Append(line?.ToString());
        //            }
        //        }
        //    }

        //    return sourceBuilder.ToString();
        //}
    }
}
