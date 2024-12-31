using CardHub.Domain.Games.Shared;

namespace CardHub.Domain.Games.Une.Card;

public interface IUneCard : ICard
{
    int? Number { get; init; }
    int DrawValue { get; init; }
    UneAction Action { get; init; }
    UneColor Color { get; init; }
    
    bool PickColor { get; init; }
    bool PickPlayer { get; init; }
}