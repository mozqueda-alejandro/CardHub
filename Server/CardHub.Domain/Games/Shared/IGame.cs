namespace CardHub.Domain.Games.Shared;

public interface IGame
{
    Guid Id { get; }
    int MaxPlayers { get; set; }
}