using CardHub.Games.Common;
using Microsoft.AspNetCore.SignalR;

namespace CardHub.Games.Une;

public class UneHub : Hub<IBaseClient>, IBaseHub
{
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

    public async Task Ping() => await Clients.Caller.Pong();
}