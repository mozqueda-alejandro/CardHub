using CardHub.Domain.Games.Shared;
using CardHub.Domain.Games.Une.Card;

namespace CardHub.Domain.Games.Une.Entities;

public class UnePlayer : CardPlayer<UneCard>
{
    public UnePlayer(string name) : base(name) { }
}