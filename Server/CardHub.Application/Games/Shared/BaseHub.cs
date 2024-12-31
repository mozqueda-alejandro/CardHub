using System.Security.Claims;
using CardHub.Domain.Games.Shared;
using Microsoft.AspNetCore.SignalR;

namespace CardHub.Application.Games.Shared;

public abstract class BaseHub<THubClient, TGame> : Hub<THubClient>
    where THubClient : class, IBaseClient
    where TGame : IGame
{
    protected IGameService<TGame> GameService;

    // protected BaseHub(IGameService<TGame> gameService)
    // {
    //     GameService = gameService;
    // }

    public async Task<bool> TryJoinGame()
    {
        // var game = serviceProvider.GetService<UneGame>();
        return false;
    }

    public async Task SendAvatarsToGroup(string[] avatars)
    {
        // await hubContext.Clients.Group(RoomId!).ReceiveAvatars(avatars);
    }

    public async Task SendMessageToGroup(string roomId, Message message)
    {
        await Clients.Group(roomId).ReceiveMessage(message);
    }

    public async Task SendMessage(IEnumerable<string> connectionIds, Message message)
    {
        await Clients.Clients(connectionIds).ReceiveMessage(message);
    }

    // public async Task SendMessage(string connectionId, Message message)
    // {
    //     await Clients.Client(connectionId).ReceiveMessage(message);
    // }

    public async Task KickPlayer()
    {
        throw new NotImplementedException();
    }

    protected TGame Game
    {
        get
        {
            GameService.TryGetGame(out var baseGame);
            return baseGame ?? throw new NullReferenceException();
        }
    }

    public async Task BasePing()
    {
        var clientId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var sessionId = Context.User?.FindFirst("session_id")?.Value;
        await Clients.Caller.BasePong();
    }
}