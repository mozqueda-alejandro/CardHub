using CardHub.Games.Common;
using CardHub.Games.Une.Card;

namespace CardHub.Games.Une.Entities;

public class UnePlayer : CardPlayer<UneCard>
{
    public UnePlayer(string name) : base(name) { }
}