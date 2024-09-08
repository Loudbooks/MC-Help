using System.Text.Json;
using System.Text.RegularExpressions;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using LogInspect.Inspectors.Latest;
using LogInspect.Pages;

namespace LogInspect.Modules;

[CommandContextType(InteractionContextType.BotDm, InteractionContextType.PrivateChannel, InteractionContextType.Guild)]
[IntegrationType(ApplicationIntegrationType.UserInstall)]
// ReSharper disable once UnusedType.Global
public partial class LogCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly Dictionary<ulong, Paginator> PageMap = new();
    private static readonly McLogInspector McLogInspector = new();

    [SlashCommand("mclog", "Inspect a Minecraft log file")]
    public async Task LatestAsync(string link, bool ephemeral = true)
    {
        await DeferAsync(ephemeral: ephemeral);
        
        try
        {
            var log = HttpHandler.FetchData(link);
            
            var message = (RestInteractionMessage) await ModifyOriginalResponseAsync(properties =>
            {
                properties.Content = "Inspecting log...";
            });
            
            var inspectedArray = McLogInspector.InspectWithPages(log.Split("\n"));
            
            for (var i = 0; i < inspectedArray.Length; i++)
            {
                inspectedArray[i] = $"""
                                     Viewing stacktrace **{i + 1}** of **{inspectedArray.Length}**:
                                     
                                     ```{inspectedArray[i]}```
                                     """;
            }
            
            var pageOne = GeneratePageOne(log.Split("\n"), inspectedArray.Length);
            var newInspectedArray = new string[inspectedArray.Length + 1];
            newInspectedArray[0] = pageOne;
            inspectedArray.CopyTo(newInspectedArray, 1);
            
            PageMap[Context.User.Id] = new Paginator(message, newInspectedArray);
            await PageMap[Context.User.Id].InitiatePage();
        }
        catch (Exception e)
        {
            await ModifyOriginalResponseAsync(properties =>
            {
                var reason = e switch
                {
                    HttpRequestException => "Failed to fetch log.",
                    UriFormatException => "Invalid link.",
                    IndexOutOfRangeException => "Failed to inspect due to a malformed link.",
                    _ => "Failed to inspect."
                };
                
                properties.Content = $"""
                                      {reason}

                                      ```
                                      {e.Message}
                                      ```
                                      """;
            });        
        }
    }
    
    [ComponentInteraction("first")]
    public async Task FirstPageAsync()
    {
        if (PageMap.TryGetValue(Context.User.Id, out var paginator))
        {
            await paginator.FirstPageAsync();
            
            await DeferAsync();
        }
    }
    
    [ComponentInteraction("previous")]
    public async Task PreviousPageAsync()
    {
        if (PageMap.TryGetValue(Context.User.Id, out var paginator))
        {
            await paginator.PreviousPageAsync();
            
            await DeferAsync();
        }
    }
    
    [ComponentInteraction("next")]
    public async Task NextPageAsync()
    {
        Console.WriteLine("Next");
        
        if (PageMap.TryGetValue(Context.User.Id, out var paginator))
        {
            try {
                await paginator.NextPageAsync();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            await DeferAsync();
        }
    }
    
    [ComponentInteraction("last")]
    public async Task LastPageAsync()
    {
        if (PageMap.TryGetValue(Context.User.Id, out var paginator))
        {
            await paginator.LastPageAsync();
            
            await DeferAsync();
        }
    }

    private string GeneratePageOne(string[] lines, int stacktraceAmount)
    {
        var newLines = new List<string> { "__**Log Information:**__" };

        var minecraftVersion = "";
        var timeCreated = "";
        
        try
        {
            var versionLines = lines.Where(line => line.Contains("Request version details for")).ToArray();

            if (versionLines.Length > 0)
            {
                minecraftVersion = versionLines[0].Split("]").Last().Split(": ").Last();

                timeCreated = lines[0].Split("]").First().Replace("[", "").Trim();
            }
            else
            {
                versionLines = lines.Where(line => line.Contains("--version,")).ToArray();
                
                if (versionLines.Length != 0)
                {
                    minecraftVersion = versionLines[0].Split("--version, ").Last().Split(", ").First();
                    
                    var secondLine = lines[1];
                    timeCreated = secondLine.Split(",").First();
                }
            }
            
            if (timeCreated != "")
            {
                newLines.Add($"Time Created: `{timeCreated}`");
            }
            newLines.Add("");

            if (minecraftVersion != "")
            {
                newLines.Add($"Minecraft Version: `{minecraftVersion}`");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        try
        {
            var javaVersionLines = lines.Where(line => line.Contains("Compatibility level set to")).ToArray();
            
            if (javaVersionLines.Length != 0)
            {
                var javaVersion = javaVersionLines[0].Split("Compatibility level set to ").Last();
                newLines.Add($"Java Version: `{javaVersion}`");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        if (minecraftVersion == "" && timeCreated == "")
        {
            newLines =
            [
                "Failed to retrieve log information."

            ];

            return string.Join("\n", newLines);
        }
        
        newLines.Add("");
        newLines.Add($"Fatal Errors: `{lines.Count(line => line.Contains("/FATAL"))}`");
        newLines.Add($"Errors: `{lines.Count(line => line.Contains("/ERROR"))}`");
        newLines.Add($"Warnings: `{lines.Count(line => line.Contains("/WARN"))}`");
        
        if (stacktraceAmount != 0)
        {
            newLines.Add($"Stacktraces: `{stacktraceAmount}`");
        }

        return string.Join("\n", newLines);
    }
}