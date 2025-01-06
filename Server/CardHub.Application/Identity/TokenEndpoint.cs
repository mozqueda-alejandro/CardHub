using CardHub.Application.Games.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CardHub.Application.Identity;

public record CreateGBTokenRequest(GameType GameType);
public record CreatePlayerTokenRequest(int RoomPin);
public record UpdateTokenRequest(IClient Client);

public static class TokenEndpoint
{
    public static void MapTokenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tokens/gameboard", CreateGBToken);
        app.MapPost("/api/v1/tokens/player", CreatePlayerToken);
        app.MapPost("/api/v1/tokens/update", UpdateToken);
    }

    private static async Task<IResult> CreateGBToken(
        [FromBody] CreateGBTokenRequest request,
        IClientManager clientManager,
        TokenProvider tokenProvider)
    {
        var client = clientManager.CreateGB();
        var token = tokenProvider.Create(client);

        return Results.Ok(token);
    }

    private static async Task<IResult> CreatePlayerToken(
        [FromBody] CreatePlayerTokenRequest request,
        IClientManager clientManager,
        TokenProvider tokenProvider)
    {
        if (!clientManager.CreatePlayer(out var client, request.RoomPin) || client == null)
        {
            return Results.BadRequest($"Player could not be created for room: {request.RoomPin}");
        }

        var token = tokenProvider.Create(client);
        return Results.Ok(token);
    }

    private static async Task<IResult> UpdateToken(
        [FromBody] UpdateTokenRequest request,
        TokenProvider tokenProvider)
    {
        var token = tokenProvider.Create(request.Client);
        return Results.Ok(token);
    }
}