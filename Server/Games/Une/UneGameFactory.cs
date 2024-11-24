using CardHub.Games.Une.Entities;

namespace CardHub.Games.Une;

public class UneGameFactory(Func<UneGame> factory)
{
    public UneGame Create(List<UnePlayer> players)
    {
        var game = factory();
        game.AddPlayers(players);
        
        return game;
    }
}