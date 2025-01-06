using Microsoft.AspNetCore.SignalR;

namespace CardHub.Application.Identity;

public class NameUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.Identity?.Name;
    }
}