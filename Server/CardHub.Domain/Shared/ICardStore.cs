namespace CardHub.Domain.Shared;

public interface ICardStore<out TCard> where TCard : ICard
{
    TCard Get(int id);
}