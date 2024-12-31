using CardHub.Domain.Games.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CardHub.Application;


public interface IGameService<TGame> where TGame : IGame
{
    bool TryCreateGame();
    bool TryGetGame(out TGame? game, string? roomId = null);
}

public class GameService<TGame>(
    // IClientManager clientManager,
    // IDictionary<string, TGame> games,
    // IServiceProvider serviceProvider
    )
    where TGame : IGame
{
    public bool TryCreateGame()
    {
        // var roomId = "";
        // var newGame = serviceProvider.GetService<TGame>();
        // if (newGame == null) return false;
        //
        // return games.TryAdd(roomId, newGame);

        return false;
    }

    public bool TryGetGame(out TGame? game, string? roomId = null)
    {
        // roomId ??= RoomId;
        game = default;

        if (string.IsNullOrEmpty(roomId))
        {
            Console.WriteLine("TryGetGame - roomId is null");
            return false;
        }
        
        // if (!games.TryGetValue(roomId, out var baseGame))
        // {
        //     Console.WriteLine("TryGetGame - Game not found for the given roomId");
        //     return false;
        // }
        
        // if (baseGame is not TGame)
        // {
        //     Console.WriteLine("TryGetGame - Found game, but it is not of type TGame");
        //     return false;
        // }

        // game = baseGame;
        return true;
    }
}