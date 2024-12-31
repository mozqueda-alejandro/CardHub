using Microsoft.AspNetCore.Mvc;
using CardHub.Application;
using CardHub.Application.Games.Shared;

namespace CardHub.Web.Identity;

public static class CreateRoomEndpoint
{
    public static void MapRoomEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/room", Handle);
        app.MapGet("/api/v1/test", Get).RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromBody] GameType request,
        IClientManager clientManager,
        TokenProvider tokenProvider,
        CancellationToken cancellationToken)
    {
        var id = clientManager.GenerateId();
        var token = tokenProvider.Create(new ClientIdentifier(ClientId: id, SessionId: id));
        
        return Results.Ok(token);
    }
    
    private static async Task<IResult> Get(
        CancellationToken cancellationToken)
    {
        var str = "Hello, World!";
        return Results.Ok(str);
    }
}