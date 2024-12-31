using System.Collections;

namespace CardHub.Domain.Games.Shared;

public interface IDeck<TCard> : IEnumerable<TCard> where TCard : ICard
{
    void Add(TCard card);
    void AddRange(IEnumerable<TCard> cards);
    void Shuffle();
    TCard? Draw();
    List<TCard> Draw(int toDraw);
    TCard? Current { get; }
    
    new IEnumerator<TCard> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}