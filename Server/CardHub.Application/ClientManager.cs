namespace CardHub.Application;

public interface IClient
{
    Guid ClientId { get; init; }
    Guid RoomId { get; init; }
    string DisplayName { get; init; }
}

public record Client : IClient
{
    public Guid ClientId { get; init; }
    public Guid RoomId { get; init; }
    public string DisplayName { get; init; }

    private Client(Guid clientId, Guid roomId, string displayName = "")
    {
        ClientId = clientId;
        RoomId = roomId;
        DisplayName = displayName;
    }
 
    public static Client CreateGB(Guid clientId, Guid roomId) => new(clientId, roomId);
    public static Client CreatePlayer(Guid clientId, Guid roomId, string displayName) => new(clientId, roomId, displayName);
    
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
    Guid GenerateId();
    bool Add(IClient context);
    IClient? Get(Guid clientId);
}

// Persists a user to the connection ID
// https://learn.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-2.1#get-an-instance-of-ihubcontext-in-middleware
public class ClientManager
// (
//     IDictionary<Guid, Room> rooms, // RoomId -> Room
//     IDictionary<Guid, IClient> contextStore) // ClientId -> Context
    : IClientManager
{
    public Guid GenerateId()
    {
        var id = Guid.NewGuid();
        // if (!connections.TryAdd(id, string.Empty)) throw new InvalidOperationException();

        return id;
    }

    public bool Add(IClient context)
    {
        var roomId = context.RoomId;

        // if (!rooms.TryGetValue(roomId, out var room))
        {
            Console.WriteLine($"Room {roomId} does not exist");
            return false;
        }

        //  


        return false;
    }

    public IClient? Get(Guid clientId)
    {
        return default;
    }
}