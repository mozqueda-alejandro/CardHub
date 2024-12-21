namespace CardHub.Games.Common;

public interface IClient
{
    Guid ClientId { get; init; }
    string ConnectionId { get; init; }
    string RoomId { get; init; }
    string? DisplayName { get; init; }
}

public class Client : IClient
{
    public required Guid ClientId { get; init; }
    public required string ConnectionId { get; init; }
    public required string RoomId { get; init; }
    public required string? DisplayName { get; init; }
}

public class Room
{
    public IClient Gameboard { get; init; }
    public List<IClient> Players { get; } = [];

    public Room(IClient gameboard)
    {
        Gameboard = gameboard;
    }
    
    
}

public interface IClientManager
{
    IClient Current { get; }
    IClient? Get(string clientId);
    
    string CurrentRoom { get; }
}

public class ClientManager(
    IDictionary<string, Room> _rooms,
    IDictionary<Guid, string> _connections,
    IDictionary<string, IClient> _contextStore)
{
    // ConnectionId -> Context
    // ClientId -> ConnectionId
    // RoomId -> Room
    
    public bool AddClient(IClient context)
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