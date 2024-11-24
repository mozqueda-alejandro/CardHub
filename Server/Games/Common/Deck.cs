namespace CardHub.Games.Common;

public class Deck<TCard> : IDeck<TCard> where TCard : class
{
    private List<TCard> _cards = [];
    private readonly List<TCard> _discardPile = [];

    public Deck() { }

    public Deck(IEnumerable<TCard> cards)
    {
        _cards = cards.ToList();
    }

    public void Add(TCard card)
    {
        _cards.Add(card);
    }

    public void AddRange(IEnumerable<TCard> cards)
    {
        _cards.AddRange(cards);
    }

    public void Shuffle()
    {
        _cards = _cards.OrderBy(_ => Random.Shared.Next()).ToList();
    }

    public TCard? Draw()
    {
        if (Count == 0)
        {
            if (_discardPile.Count == 0)
            {
                Console.WriteLine("No cards to draw from the deck.");
                return default;
            }
            
            ReclaimCards();
        }

        var drawnCard = _cards.ElementAt(0);
        _cards.RemoveAt(0);
        return drawnCard;
    }

    public List<TCard> Draw(int toDraw)
    {
        if (toDraw < 1) throw new ArgumentException("Attempted to draw less than 1 card from the deck.");

        var totalDrawable = Count + _discardPile.Count;
        if (toDraw > totalDrawable)
        {
            Console.WriteLine($"Attempted to draw {toDraw} cards, but only {_cards.Count} card(s) can be drawn.");
            return [];
        }

        var drawnCards = new List<TCard>();
        if (toDraw > Count)
        {
            drawnCards.AddRange(_cards);
            _cards.Clear();

            ReclaimCards();
            drawnCards.AddRange(_cards.Take(toDraw - drawnCards.Count));
        }
        else
        {
            drawnCards.AddRange(_cards.Take(toDraw));
            _cards.RemoveRange(0, toDraw);
        }

        return drawnCards;
    }

    public void Discard(TCard card)
    {
        _discardPile.Add(card);
    }

    public void Discard(IEnumerable<TCard> cards)
    {
        _discardPile.AddRange(cards);
    }

    public TCard? Current => _discardPile.Last();

    public int Count => _cards.Count;

    private void ReclaimCards()
    {
        _cards.AddRange(_discardPile.GetRange(0, _discardPile.Count - 1));
        _discardPile.RemoveRange(0, _discardPile.Count - 1);
        Shuffle();
    }
}