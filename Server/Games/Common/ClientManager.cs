namespace CardHub.Games.Common;

public interface IClient
{
    Guid ClientId { get; init; }
    string ConnectionId { get; init; }
    string RoomId { get; init; }
}

public interface IPlayerContext : IClient
{
    string DisplayName { get; init; }
}

public class Client : IClient
{
    public required Guid ClientId { get; init; }
    public required string ConnectionId { get; init; }
    public required string RoomId { get; init; }
}

public class PlayerContext : Client, IPlayerContext
{
    public required string DisplayName { get; init; }
}

public class Room
{
    public IClient Gameboard { get; init; }
    public List<IPlayerContext> Players { get; } = [];

    public Room(IClient gameboard)
    {
        Gameboard = gameboard;
    }
    
    
}

public class ClientManager(
    IDictionary<string, Room> _rooms,
    IDictionary<Guid, string> _connections,
    IDictionary<string, IPlayerContext> _contextStore)
{
    // ConnectionId -> Context
    // ClientId -> ConnectionId
    // RoomId -> Room
    
    public bool AddPlayer(IPlayerContext context)
    {
        var roomId = context.RoomId;
        
        // Room does not exist
        if (!_rooms.TryGetValue(roomId, out var room))
        {
            Console.WriteLine($"Room {roomId} does not exist");
            return false;
        }
        
        //  
        

        return false;
    }

    // public IPlayerContext Current
    // {
    //     get
    //     {
    //         
    //     }
    // }

    public string? RoomId()
    {

        return "";
    }
    
}