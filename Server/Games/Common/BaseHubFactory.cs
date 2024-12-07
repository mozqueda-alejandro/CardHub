using Microsoft.AspNetCore.SignalR;

namespace CardHub.Games.Common;

public class BaseHubFactory<THub>(
    IServiceProvider serviceProvider)
    where THub : Hub<IBaseClient>
{
    public BaseHub<THub> Create(HubCallerContext userContext)
    {
        var baseHub = serviceProvider.GetRequiredService<BaseHub<THub>>();
        // baseHub.UserContext = userContext;
        // if (baseHub == null) Console.WriteLine("so sad is null");
        // else Console.WriteLine("Not so sad: " + baseHub.RoomId ?? "haah");
        //
        return baseHub;
    }
}