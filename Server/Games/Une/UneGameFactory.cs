using CardHub.Games.Une.Entities;

namespace CardHub.Games.Une;

public class UneGameFactory(Func<UneGame> factory)
{
    public UneGame Create(List<UnePlayer> players, UneRules rules)
    {
        var game = factory();
        game.Initialize(players, rules);
        
        return game;
    }
}