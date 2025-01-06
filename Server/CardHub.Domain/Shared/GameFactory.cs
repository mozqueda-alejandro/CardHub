namespace CardHub.Domain.Shared;

public class GameFactory<TGame>(
    Func<TGame> factory)
    where TGame : IGame
{
    public TGame Create() {
        var game = factory();

        return game;
    }
}