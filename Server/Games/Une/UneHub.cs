using CardHub.Games.Common;
using CardHub.Games.Une.Entities;
using Microsoft.AspNetCore.SignalR;

namespace CardHub.Games.Une;

public class UneHub : Hub<IBaseClient>, IBaseHub
{
    private BaseHub<UneHub> _base;
    private IGameService<UneGame> _gameService;

    public UneHub(BaseHub<UneHub> baseHub, IGameService<UneGame> gameService)
    {
        _base = baseHub;
        _gameService = gameService;
    }

    public Task StartGame()
    {
        throw new NotImplementedException();
    }

    public Task EndGame()
    {
        throw new NotImplementedException();
    }

    public Task Pause()
    {
        throw new NotImplementedException();
    }

    public Task Resume()
    {
        throw new NotImplementedException();
    }

    public Task KickPlayer()
    {
        throw new NotImplementedException();
    }

    public Task RestartGame()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> JoinGame(ClientConnection connection)
    {
        if (!_gameService.TryGetGame(out var game, connection.RoomId))
        {
            // _base.RoomId = connection.RoomId;
        }
        
        return false;
    }

    public Task GetStateGB()
    {
        throw new NotImplementedException();
    }

    public Task GetStatePlayer()
    {
        throw new NotImplementedException();
    }

    public async Task Ping() => await Clients.Caller.Pong();
    public async Task BasePing() => await _base.BasePing();

    #region Helpers

    #endregion
}