namespace CardHub.Games.Common;

public class CardPlayer<TCard> : IPlayer where TCard : ICard
{
    public string Name { get; init; }
    public List<TCard> Hand { get; }  
    
    protected CardPlayer(string name)
    {
        Name = name;
        Hand = [];
    }
    
    protected CardPlayer(string name, IEnumerable<TCard> cards)
    {
        Name = name;
        Hand = [..cards];
    }
    
    public bool Add(TCard card)
    {
        if (Hand.Any(c => c.Id == card.Id)) return false;
        
        Hand.Add(card);
        return true;
    }
    
    public bool AddRange(IEnumerable<TCard> cards)
    {
        if (Hand.Any(c => cards.Any(toAdd => toAdd.Id == c.Id))) return false;
        
        Hand.AddRange(cards);
        return true;
    }
    
    public TCard? Get(int cardId)
    {
        return Hand.Find(c => c.Id == cardId);
    }

    public TCard[] GetAll(int[] cardIds)
    {
        var toGetIds = new HashSet<int>(cardIds);
        var toGet = Hand.Where(c => toGetIds.Contains(c.Id)).ToArray();
        if (toGet.Length != cardIds.Length) return [];
        
        return toGet;
    }

    public bool Play(int cardId)
    {
        var toRemove = Hand.FirstOrDefault(c => c.Id == cardId);
        if (toRemove == null) return false;

        Hand.Remove(toRemove);
        return true;
    }

    public bool Play(int[] cardIds)
    {
        if (cardIds.Length == 0) return false;

        var toRemoveIds = new HashSet<int>(cardIds);
        var toRemove = Hand.Where(c => toRemoveIds.Contains(c.Id)).ToArray();
        if (toRemove.Length != cardIds.Length) return false;

        Hand.RemoveAll(c => toRemoveIds.Contains(c.Id));
        return true;
    }

    public TCard? GetRandomCard()
    {
        if (Hand.Count == 0) return default;
        
        var random = new Random();
        var index = random.Next(0, Hand.Count);
        var card = Hand[index];
        Hand.RemoveAt(index);
        return card;
    }
    
    public void Clear() => Hand.Clear();
    
    public int Count => Hand.Count;
    
    public override bool Equals(object? obj)
    {
        if (obj is not CardPlayer<TCard> toCompare)
            return false;
        
        return Name == toCompare.Name; 
    }
    
    public override int GetHashCode() => Name.GetHashCode();
}