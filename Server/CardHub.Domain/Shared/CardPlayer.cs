namespace CardHub.Domain.Shared;

public class CardEqualityComparer<TCard> : IEqualityComparer<TCard>
{
    public bool Equals(TCard? x, TCard? y)
    {
        if (x is null || y is null) return false;

        return false;
    }
    
    public int GetHashCode(TCard obj) => obj.GetHashCode();
}

public class CardPlayer<TCard> : IPlayer
{
    public string Name { get; init; }
    public List<TCard> Hand { get; } = [];
    public int CardLimit { get; init; } = 20;
    private IEqualityComparer<TCard> _comparer;
    
    protected CardPlayer(string name)
    {
        Name = name;
    }
    
    protected CardPlayer(string name, IEnumerable<TCard> cards)
    {
        Name = name;
        Hand = [..cards];
    }
    
    public bool Add(TCard card)
    {
        if (Hand.Any(c => _comparer.Equals(card, c))) return false;
        
        Hand.Add(card);
        return true;
    }
    
    public bool AddRange(IEnumerable<TCard> cards)
    {
        if (Hand.Any(c => cards.Any(toAdd => _comparer.Equals(toAdd, c)))) return false;
        
        Hand.AddRange(cards);
        return true;
    }
    
    public TCard? Get(int cardId)
    {
        // return Hand.Find(c => c.Id == cardId);
        return Hand.Find(c => true);
    }

    public List<TCard> Select(List<int> cardIds)
    {
        var toGetIds = new HashSet<int>(cardIds);
        // var toGet = Hand.Where(c => toGetIds.Contains(c.Id)).ToList();
        var toGet = Hand.Where(c => toGetIds.Contains(default)).ToList();
        if (toGet.Count != cardIds.Count) return [];
        
        return toGet;
    }

    public bool Play(int cardId)
    {
        // var toRemove = Hand.FirstOrDefault(c => c.Id == cardId);
        var toRemove = Hand.FirstOrDefault(c => true);
        if (toRemove == null) return false;

        Hand.Remove(toRemove);
        return true;
    }

    public bool Play(List<int> cardIds)
    {
        if (cardIds.Count == 0) return false;

        var toRemoveIds = new HashSet<int>(cardIds);
        // var toRemove = Hand.Where(c => toRemoveIds.Contains(c.Id)).ToArray();
        var toRemove = Hand.Where(c => toRemoveIds.Contains(default)).ToArray();
        if (toRemove.Length != cardIds.Count) return false;

        // Hand.RemoveAll(c => toRemoveIds.Contains(c.Id));
        return true;
    }

    public TCard? GetRandomCard()
    {
        if (Hand.Count == 0) return default;
        
        var random = new Random();
        var index = random.Next(0, Hand.Count);
        return Hand[index];
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