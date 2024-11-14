using CardHub.Games.Common;

namespace CardHub.Games.Une;

public interface IUneOrder<TPlayer> where TPlayer : IPlayer
{
    TPlayer Current { get; }
    TPlayer First { get; }
    TPlayer Last { get; }
    TPlayer? Find(string playerName);
    TPlayer GetOffset(int offset = 1);
    
    TPlayer? Iterate(string playerName, int offset = 0);
    TPlayer Iterate(int offset = 1);
    
    bool Add(TPlayer player);
    bool Remove(string playerName);
    void ReverseDirection();
    void Shuffle();
}