using CardHub.Domain.Games.Shared;

namespace CardHub.Domain.Games.Une.Entities;

public enum UneStackRule
{
    Equal,
    EqualOrGreater,
    None
}

public class UneRuleSet : IGameRuleSet
{
    // First playable card drawn during a player's turn must be played
    public required bool ForcePlay { get; init; }
    
    // Player may draw more than one card during their turn
    public required bool FreeDraw { get; init; }
    
    // Any player with the same card as the Current card, can play it immediately, even if it's not their turn
    public required bool JumpIn { get; init; }
    
    // Once a player has 25 or more cards in their hand, they are out of the game
    public required bool Mercy { get; init; }
    
    // Player must swap hands with another player of their choice after playing a 7 Number card
    public required bool SevensSwap { get; init; }
    
    // All players must pass their hand to the next player in the Order direction after a 0 Number card is played
    public required bool ZerosPass { get; init; }
    
    // Allows for stacking if DrawAmounts are equal, greater than or equal to, or no stacking
    public required UneStackRule UneStackRule { get; init; }
    
    public int BotsCount { get; init; }
}

/*
ForcePlay, FreeDraw (DETERMINISTIC)
DrawUntil 
// var drawnCards = DrawUntilValid();
player.AddRange(drawnCards);
player.PlayCard(drawnCards.Last);

NoForcePlay, FreeDraw
NoLimitDraw
// player.Draw();
player.Draw();
...
player.PlayCard(c);
// or punish player

ForcePlay, NoFreeDraw (DETERMINISTIC)
StandardDraw
// var drawnCard = player.Draw();
if (IsPlayable(drawnCard))
{
    player.PlayCard(drawnCard);
}
else
{
    // punish player
}

NoForcePlay, NoFreeDraw (DETERMINISTIC)
RelaxedDraw // SendCard
// var drawnCard = player.Draw();
*/