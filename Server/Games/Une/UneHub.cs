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

    public Task<bool> StartGame()
    {
        // var game = service.GetGame()
        // if (!game.CanStart()) return false;
        
        // game.Start();
        
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

    public async Task RestartGame()
    {
        await _base.RestartGame();
    }

    public async Task<bool> JoinGame(ClientConnection connection)
    {
        var joinSuccess = await _base.TryJoinGame();
        return joinSuccess;
    }
    
    // public async Task Handle
        
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