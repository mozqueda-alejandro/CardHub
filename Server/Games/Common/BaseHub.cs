using Microsoft.AspNetCore.SignalR;

namespace CardHub.Games.Common;


public class BaseHub<THub>(
    IHubContext<THub, IBaseClient> hubContext,
    IServiceProvider serviceProvider) : IBaseHub
    where THub : Hub<IBaseClient>
{
    public async Task<bool> TryJoinGame()
    {
        
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
        // await hubContext.Clients.Group(RoomId!).BasePong();
    }
}