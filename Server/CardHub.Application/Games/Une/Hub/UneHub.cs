using CardHub.Application.Games.Shared;
using CardHub.Application.Games.Shared.Hub;
using CardHub.Domain.Une.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CardHub.Application.Games.Une.Hub;

public interface IUneClient : IBaseClient
{
    
}

public class UneHub : BaseHub<IUneClient, UneGame>
{
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

    public async Task PlayCard(int cardId)
    {
        
    }
    
    public async Task Test()
    {
        Console.WriteLine("ClientId: ", Client.ClientId);
        Console.WriteLine("RoomId: ", Client.RoomId);
    }

    public async Task RestartGame() { }
    
    #region Utils
    
    public async Task Ping()
    {
        await Test();
        await Clients.All.Pong();
    }

    #endregion
}