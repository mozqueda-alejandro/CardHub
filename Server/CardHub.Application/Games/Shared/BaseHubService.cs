using CardHub.Domain.Games.Une.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace CardHub.Application.Games.Shared;


public class BaseHubService<THub>(
    IHubContext<THub, IBaseClient> hubContext,
    IServiceProvider serviceProvider) : IBaseHub
    where THub : Hub<IBaseClient>
{
    public async Task<bool> TryJoinGame()
    {
        var game = serviceProvider.GetService<UneGame>();
        return false;
    }
    
    public async Task SendAvatarsToGroup(string[] avatars)
    {
        // await hubContext.Clients.Group(RoomId!).ReceiveAvatars(avatars);
    }

    public async Task SendMessageToGroup(string roomId, Message message)
    {
        await hubContext.Clients.Group(roomId).ReceiveMessage(message);
    }

    public async Task SendMessage(IEnumerable<string> connectionIds, Message message)
    {
        await hubContext.Clients.Clients(connectionIds).ReceiveMessage(message);
    }

    public async Task SendMessage(string connectionId, Message message)
    {
        await hubContext.Clients.Client(connectionId).ReceiveMessage(message);
    }

    public async Task RestartGame()
    {
        
    }

    public async Task BasePing()
    {
        Console.WriteLine("BasePing called");
        await hubContext.Clients.All.BasePong();
    }
}