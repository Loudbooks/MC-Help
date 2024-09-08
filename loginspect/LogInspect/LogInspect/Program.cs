using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;

namespace LogInspect;

public static class Program
{
    private static DiscordSocketClient? _client;
    private static InteractionService? _interactionService;
    private static IServiceProvider? _services;

    public static string DataPath { get; private set; } = null!;

    public static async Task Main()
    {
        Env.Load();
        
        var token = Env.GetString("DISCORD_TOKEN");
        var dataPath = Env.GetString("DATA_PATH");
        
        if (token == null || dataPath == null)
        {
            throw new Exception("Failed to load environment variables");
        }
        
        DataPath = dataPath;
        
        _client = new DiscordSocketClient();
        _interactionService = new InteractionService(_client);
        _client.Log += Log;
        
        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_interactionService)
            .BuildServiceProvider();

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _client.Ready += RegisterCommands;
        _client.InteractionCreated += HandleInteraction;
        
        HttpHandler.FetchHjtFlags(DataPath);
        
        await Task.Delay(-1);
    }
    
    private static async Task RegisterCommands()
    {
        Console.WriteLine("Registering commands...");
        
        if (_interactionService == null)
        {
            throw new Exception("Interaction service is null");
        }
        
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        await _interactionService.RegisterCommandsGloballyAsync();
        
        Console.WriteLine("Commands registered!");
    }
    
    private static async Task HandleInteraction(SocketInteraction arg)
    {
        var context = new SocketInteractionContext(_client, arg);
        
        if (_interactionService == null)
        {
            throw new Exception("Interaction service is null");
        }
        
        await _interactionService.ExecuteCommandAsync(context, _services);
    }
    
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}