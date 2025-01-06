using CardHub.Domain.Shared;

namespace CardHub.Domain.Une.Card;

public interface IUneCard : ICard
{
    int? Number { get; init; }
    int DrawValue { get; init; }
    UneAction Action { get; init; }
    UneColor Color { get; init; }
    
    bool PickColor { get; init; }
    bool PickPlayer { get; init; }
}