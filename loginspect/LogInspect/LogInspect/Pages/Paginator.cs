using Discord;
using Discord.Rest;

namespace LogInspect.Pages;

public class Paginator(RestInteractionMessage message, string[] pages)
{
    private int _currentPage;
    
    public async Task InitiatePage()
    {
        await message.ModifyAsync(properties =>
        {
            properties.Content = pages[_currentPage];

            if (pages.Length > 1)
            {
                properties.Components = new ComponentBuilder()
                    .WithButton("First", "first", ButtonStyle.Danger, disabled: _currentPage == 0)
                    .WithButton("Previous", "previous", ButtonStyle.Danger, disabled: _currentPage == 0)
                    .WithButton("Next", "next", ButtonStyle.Danger, disabled: _currentPage == pages.Length - 1)
                    .WithButton("Last", "last", ButtonStyle.Danger, disabled: _currentPage == pages.Length - 1)
                    .Build();
            }
        });
    }
    
    public async Task NextPageAsync()
    {
        if (_currentPage < pages.Length - 1)
        {
            _currentPage++;
            await InitiatePage();
        }
    }
    
    public async Task PreviousPageAsync()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            await InitiatePage();
        }
    }
    
    public async Task FirstPageAsync()
    {
        _currentPage = 0;
        await InitiatePage();
    }
    
    public async Task LastPageAsync()
    {
        _currentPage = pages.Length - 1;
        await InitiatePage();
    }
}