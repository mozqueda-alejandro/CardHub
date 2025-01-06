using System.Collections;

namespace CardHub.Domain.Shared;

public enum Direction
{
    Forward,
    Backward
}

public class Order<TPlayer> : IEnumerable where TPlayer : IPlayer
{
    private List<string> _names;
    private Dictionary<string, TPlayer> _players;
    private int _currentIndex;
    private Direction _direction = Direction.Forward;
    private const int MaxCapacity = 8;

    public Order(IEnumerable<TPlayer> players)
    {
        var playerArray = players.ToArray();
        if (playerArray.Length > MaxCapacity) throw new ArgumentException("Too many players");
        if (playerArray.Length <= 1) throw new ArgumentException("Too few players");
        if (playerArray.Select(p => p.Name).Distinct().Count() != playerArray.Length)
            throw new ArgumentException("Player IDs must be unique");

        _players = playerArray.ToDictionary(p => p.Name);
        _names = playerArray.Select(p => p.Name).ToList();
    }

    public int Count => _players.Count;

    public TPlayer Current => _players[_names[_currentIndex]];

    public TPlayer First => _players[_names.First()];
    
    public TPlayer Last => _players[_names.Last()];

    public TPlayer? Find(string playerName)
    {
        return _players.GetValueOrDefault(playerName);
    }

    public TPlayer GetOffset(int offset = 1)
    {
        var targetIndex = ComputeIndex(offset);
        var player = _players[_names[targetIndex]];

        return player;
    }

    public TPlayer? Iterate(string playerName, int offset = 0)
    {
        var newIndex = _names.IndexOf(playerName);
        if (newIndex == -1) return default;

        _currentIndex = newIndex;
        if (offset != 0) Iterate(offset);
        return Current;
    }

    public TPlayer Iterate(int offset = 1)
    {
        var targetIndex = ComputeIndex(offset);
        var player = _players[_names[targetIndex]];
        _currentIndex = targetIndex;
        return player;
    }

    public void ReverseDirection()
    {
        _direction = _direction == Direction.Forward ? Direction.Backward : Direction.Forward;
    }

    public void Shuffle()
    {
        if (_players.Count < 2) return;
        
        var random = new Random();
        _names = _names.OrderBy(x => random.Next()).ToList();
        _direction = Direction.Forward;
    }

    public bool Add(TPlayer player)
    {
        var name = player.Name;
        if (!_players.TryAdd(name, player)) return false;

        _names.Add(name);
        return true;
    }

    public bool Remove(string playerName)
    {
        if (!_players.ContainsKey(playerName) && !_names.Contains(playerName)) return false;
        
        if (playerName == Current.Name) Iterate();
        return _players.Remove(playerName) && _names.Remove(playerName);
    }

    public void Clear()
    {
        _players.Clear();
        _names.Clear();
    }

    private int ComputeIndex(int offset)
    {
        return _direction switch
        {
            Direction.Forward => (_currentIndex + offset) % Count,
            Direction.Backward => (_currentIndex - offset) % Count,
            _ => throw new ArgumentOutOfRangeException(nameof(_direction))
        };
    }

    public IEnumerator<TPlayer> GetEnumerator()
    {
        return _players.Select(player => player.Value).GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}