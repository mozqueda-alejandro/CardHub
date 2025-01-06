using CardHub.Domain.Shared;
using CardHub.Domain.Une.Card;

namespace CardHub.Domain.Une.Entities;

public class UnePlayer : CardPlayer<UneCard>
{
    public UnePlayer(string name) : base(name) { }
}