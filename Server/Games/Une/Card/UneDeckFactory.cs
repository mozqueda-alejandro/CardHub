namespace CardHub.Games.Une.Card;

public class UneDeckFactory(IDictionary<int, UneCard> cardSet)
{
    public List<UneCard> Create()
    {
        List<UneCard> cards = [];

        var c = cardSet.Values.ToList();
        cards = c;
        
        return cards;
    }
}