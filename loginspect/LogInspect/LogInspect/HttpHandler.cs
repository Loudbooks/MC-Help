using System.Text.Json;

namespace LogInspect;

public static class HttpHandler
{
    public static string FetchData(string url)
    {
        var uri = new Uri(url);
        var baseDomain = uri.Host;
        
        var parts = uri.AbsolutePath.Split('/');

        string? rewrittenUrl;
        switch (baseDomain)
        {
            case "paste.gg":
                if (url.EndsWith("/raw"))
                {
                    rewrittenUrl = url;
                    break;
                }

                var pasteId = parts[3].Split("?")[0];
                
                var apiResponse = new HttpClient().GetAsync($"https://api.paste.gg/v1/pastes/{pasteId}/files").Result;
                var apiResponseString = apiResponse.Content.ReadAsStringAsync().Result;
                var apiResponseJson = JsonSerializer.Deserialize<JsonElement>(apiResponseString);
                var firstFile = apiResponseJson.GetProperty("result")[0].GetProperty("id").GetString();
                
                rewrittenUrl = $"https://paste.gg/p/anonymous/{pasteId}/files/{firstFile}/raw";
                break;
            case "pastebook.dev":
                if (url.EndsWith("/content"))
                {
                    rewrittenUrl = url;
                    break;
                }

                var pastebookId = parts[2].Split("?")[0];
                
                rewrittenUrl = $"https://pastebook.dev/api/get/{pastebookId}/content";
                break;
            case "mclo.gs":
                var mclogsId = parts[1];
                
                rewrittenUrl = $"https://api.mclo.gs/1/raw/{mclogsId}";
                break;
            default:
                rewrittenUrl = null;
                break;
        }

        if (rewrittenUrl == null) return "";
        
        var client = new HttpClient();
        var response = client.GetAsync(rewrittenUrl).Result;
        
        Console.WriteLine($"Fetched data from {baseDomain}.");
        
        var responseString = response.Content.ReadAsStringAsync().Result;
        
        return responseString;
    }
    
    public static void FetchHjtFlags(string dataPath)
    {
        var client = new HttpClient();
        var response = client.GetAsync("https://raw.githubusercontent.com/MinecraftHopper/MinecraftHopper/master/hjt.json").Result;
        
        var responseString = response.Content.ReadAsStringAsync().Result;
        
        File.WriteAllText($"{dataPath}/HjtFlags.json", responseString);
        
        Console.WriteLine("Fetched HJT flags.");
    }
}