namespace CardHub.Games.Common;

public class ClientConnection
{
    public required Guid ClientId { get; init; }
    public required string RoomId { get; init; }
}