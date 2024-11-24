using CardHub.Games.Une.Card;

namespace CardHub.Games.Une.Entities;

public class UneStateGB
{
    public List<UneCard> DiscardPile { get; set; }
    public List<UnePlayer> Players { get; set; }
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