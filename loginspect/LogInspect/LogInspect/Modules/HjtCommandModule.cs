using System.Text.Json;
using Discord;
using Discord.Interactions;
using LogInspect.Inspectors.HJT;

namespace LogInspect.Modules;

[CommandContextType(InteractionContextType.BotDm, InteractionContextType.PrivateChannel, InteractionContextType.Guild)]
[IntegrationType(ApplicationIntegrationType.UserInstall)]
public class HjtCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly HjtInspector HjtInspector = new();
    
    [SlashCommand("hjt", "Inspect a HiJackThis log file")]
    public async Task HiJackThisAsync([Summary(description: "The link to the log")] string link, bool ephemeral = true)
    {
        await DeferAsync(ephemeral: ephemeral);
        
        try
        {
            var log = HttpHandler.FetchData(link);

            var issues = HjtInspector.Inspect(log.Split("\n"));
            
            await ModifyOriginalResponseAsync(properties =>
            {
                properties.Content = issues;
            });
        }
        catch (Exception e)
        {
            await ModifyOriginalResponseAsync(properties =>
            {
                var reason = e switch
                {
                    HttpRequestException => "Failed to fetch log.",
                    JsonException => "Failed to deserialize HJT flags.",
                    UriFormatException => "Invalid link.",
                    IndexOutOfRangeException => "Failed to inspect due to a malformed link.",
                    KeyNotFoundException => "Failed to inspect. Are you sure the log exists?",
                    _ => "Failed to inspect."
                };
                
                Console.WriteLine(e);

                properties.Content = $"""
                                      {reason}

                                      ```
                                      {e.Message}
                                      ```
                                      """;
            });        
        }
    }
}