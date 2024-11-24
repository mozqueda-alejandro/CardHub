namespace CardHub.Games.Common;

public interface IDeck<TCard> where TCard : ICard
{
    void Add(TCard card);
    void AddRange(IEnumerable<TCard> cards);
    void Shuffle();
    TCard? Draw();
    List<TCard> Draw(int toDraw);
    TCard? Current { get; }
}