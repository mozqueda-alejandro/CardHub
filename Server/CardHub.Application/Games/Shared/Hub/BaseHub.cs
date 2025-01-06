using System.Security.Claims;
using CardHub.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CardHub.Application.Games.Shared.Hub;

[Authorize]
public abstract class BaseHub<THubClient, TGame> : Hub<THubClient>
    where THubClient : class, IBaseClient
    where TGame : IGame
{
    protected IGameManager<TGame> GameManager;
    protected IClientManager ClientManager;
    
    // public BaseHub(IGameManager<TGame> gameManager, IClientManager clientManager)
    // {
    //     GameManager = gameManager;
    //     ClientManager = clientManager;
    // }

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

    // TODO: Add auth policy for Gameboard
    // [Authorize("Gameboard")]
    public async Task KickPlayer()
    {
        throw new NotImplementedException();
    }

    protected TGame Game
    {
        get
        {
            var roomId = ExtractGuid("RoomId");
            GameManager.TryGetGame(out var baseGame, roomId);
            return baseGame ?? throw new NullReferenceException();
        }
    }

    protected IClient Client
    {
        get
        {
            var clientId = ExtractGuid(ClaimTypes.NameIdentifier);
            var client = ClientManager.Get(clientId) ?? throw new NullReferenceException();

            return client;
        }
    }

    private Guid ExtractGuid(string claimType)
    {
        var claim = Context.User?.FindFirst(claimType)?.Value;

        if (claim == null) throw new NullReferenceException($"{claimType} not resolved from JWT claims");
        if (!Guid.TryParse(claim, out var guid)) throw new FormatException($"{claim} is not a valid GUID");

        return guid;
    }

    public async Task BasePing() => await Clients.All.BasePong();
}