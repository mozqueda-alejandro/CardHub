using CardHub.Domain.Games.Une.Entities;

namespace CardHub.Domain.Games.Une;

public class UneGameFactory(Func<UneGame> factory)
{
    public UneGame Create(List<UnePlayer> players, UneRuleSet ruleSet)
    {
        var game = factory();
        game.Initialize(players, ruleSet);
        
        return game;
    }
}