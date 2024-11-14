using CardHub.Games.Une.Card;

namespace CardHub.Games.Une.Entities;

public record UneStatePlayer
{
    public required List<UneCard> Hand { get; init; }
    public UneCard? CurrentCard { get; set; }
    // public TimeSpan? TurnStart { get; set; }
    // public TimeSpan? TurnExpires { get; set; }
}