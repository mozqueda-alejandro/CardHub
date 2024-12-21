namespace CardHub.Games.Common;

public interface IGame
{
    Guid Id { get; }
    int MaxPlayers { get; set; }
}