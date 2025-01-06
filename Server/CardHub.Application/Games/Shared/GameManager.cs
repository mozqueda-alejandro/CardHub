using CardHub.Domain.Shared;

namespace CardHub.Application.Games.Shared;

public interface IGameManager<TGame> where TGame : IGame
{
    bool TryCreateGame();
    bool TryGetGame(out TGame? game, Guid roomId);
}

public class GameManager<TGame>(
    IClientManager clientManager,
    IDictionary<Guid, IGame> games,
    IServiceProvider serviceProvider) : IGameManager<TGame>
    where TGame : IGame
{
    public bool TryCreateGame()
    {
        var roomId = "";
        var newGame = serviceProvider.GetService<TGame>();
        if (newGame == null) return false;

        // return games.TryAdd(roomId, newGame);

        return false;
    }

    public bool TryGetGame(out TGame? game, Guid roomId)
    {
        game = default;

        if (!games.TryGetValue(roomId, out var baseGame))
        {
            Console.WriteLine("TryGetGame - Game not found for the given roomId");
            return false;
        }

        if (baseGame is not TGame concreteGame)
        {
            Console.WriteLine("TryGetGame - Found game, but it is not of type TGame");
            return false;
        }

        game = concreteGame;
        return true;
    }
}