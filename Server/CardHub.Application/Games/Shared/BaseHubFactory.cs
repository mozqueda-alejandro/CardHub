using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace CardHub.Application.Games.Shared;

public class BaseHubFactory<THub>(
    IServiceProvider serviceProvider)
    where THub : Hub<IBaseClient>
{
    public BaseHubService<THub> Create(HubCallerContext userContext)
    {
        var baseHub = serviceProvider.GetRequiredService<BaseHubService<THub>>();
        // baseHub.UserContext = userContext;
        // if (baseHub == null) Console.WriteLine("so sad is null");
        // else Console.WriteLine("Not so sad: " + baseHub.RoomId ?? "haah");
        //
        return baseHub;
    }
}