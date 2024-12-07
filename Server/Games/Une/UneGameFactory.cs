using CardHub.Games.Une.Entities;

namespace CardHub.Games.Une;

public class UneGameFactory(Func<UneGame> factory)
{
    public UneGame Create(List<UnePlayer> players, UneSettings settings)
    {
        var game = factory();
        game.Init(players, settings);
        
        return game;
    }
}