using CardHub.Domain.Games.Une.Card;

namespace CardHub.Domain.Games.Une.Entities;

public class UneStateGB
{
    public string CurrentPlayer { get; set; }
    public List<UneCard> DiscardPile { get; set; }
    public Dictionary<string, List<UneCard>> PlayerHands { get; set; }
}

/*
{
  "discardPile": [{}, {}],
  "turnStarted": 0,
  "turnExpires": 1,
  "players": [],
  "currentPlayer": ""
}
*/