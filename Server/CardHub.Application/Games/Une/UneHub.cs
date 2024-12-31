using CardHub.Application.Games.Shared;
using CardHub.Domain.Games.Une.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CardHub.Application.Games.Une;

[Authorize]
public class UneHub : BaseHub<IBaseClient, UneGame>
{
    public UneHub(IClientManager gameService)
        // : base(gameService)
    {
        // GameService = gameService;
        var a = gameService;
    }
    
    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"OnConnectedAsync");

        return base.OnConnectedAsync();
    }

    public async Task<bool> StartGame()
    {
        // if (!Game.CanStart()) return false;

        await Game.Start();

        throw new NotImplementedException();
    }

    public Task EndGame()
    {
        throw new NotImplementedException();
    }

    public async Task PlayCard(int cardId) { }

    public async Task RestartGame() { }
    
    #region Utils

    public string ResolveUser()
    {
        return string.Empty;
    }
    
    public async Task Ping() => await Clients.Caller.Pong();

    #endregion
}