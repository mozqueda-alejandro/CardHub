using Microsoft.AspNetCore.SignalR;

namespace CardHub.Games.Common;

public class BaseHubService<THub> : Hub where THub : Hub<IBaseClient>
{
    private readonly IHubContext<THub, IBaseClient> _context;

    public BaseHubService(IHubContext<THub, IBaseClient> context)
    {
        _context = context;
    }
    
    public async Task SendAvatarsToGroup(string roomId, string[] avatars)
    {
        await _context.Clients.Group(roomId).ReceiveAvatars(avatars);
    }

    public async Task SendMessageToGroup(string roomId, Message message)
    {
        await _context.Clients.Group(roomId).ReceiveMessage(message);
    }

    public async Task SendMessage(IEnumerable<string> connectionIds, Message message)
    {
        await _context.Clients.Clients(connectionIds).ReceiveMessage(message);
    }

    public async Task SendMessage(string connectionId, Message message)
    {
        await _context.Clients.Client(connectionId).ReceiveMessage(message);
    }
}
