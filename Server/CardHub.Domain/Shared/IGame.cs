namespace CardHub.Domain.Shared;

public interface IGame
{
    Guid Id { get; }
    int MaxPlayers { get; set; }
    bool AddPlayer(string playerId);
}