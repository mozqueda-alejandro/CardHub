using CardHub.Domain.Une.Card;

namespace CardHub.Domain.Une.Entities;

public record UneStatePlayer
{
    public required List<UneCard> Hand { get; init; }
    public UneCard? CurrentCard { get; set; }
    // public TimeSpan? TurnStart { get; set; }
    // public TimeSpan? TurnExpires { get; set; }
}