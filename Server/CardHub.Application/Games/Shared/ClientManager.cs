namespace CardHub.Application.Games.Shared;

public interface IClient
{
    Guid ClientId { get; init; }
    Guid RoomId { get; init; }
}

public record Client(Guid ClientId, Guid RoomId) : IClient;

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
    IClient CreateGB();
    bool CreatePlayer(out IClient? client, int roomPin);
    bool Remove(Guid clientId);
    IClient? Get(Guid clientId);
}

// Persists a user to the connection ID
// https://learn.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-2.1#get-an-instance-of-ihubcontext-in-middleware
public class ClientManager(
    IDictionary<int, Guid> roomPins, // RoomPin -> RoomId
    IDictionary<Guid, Room> rooms, // RoomId -> Room
    IDictionary<Guid, IClient> clients) // ClientId -> Client
    : IClientManager
{
    public IClient CreateGB()
    {
        var clientId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var gameboard = new Client(clientId, roomId);
        var room = new Room(gameboard);
        
        roomPins.Add(GeneratePin(), roomId);
        rooms.Add(roomId, room);
        clients.Add(clientId, gameboard);
        
        return new Client(clientId, roomId);
    }
    
    public bool CreatePlayer(out IClient? client, int roomPin)
    {
        if (!roomPins.TryGetValue(roomPin, out var roomId))
        {
            Console.WriteLine($"Room {roomPin} does not exist");
            client = default;
            return false;
        }

        var clientId = Guid.NewGuid();
        client = new Client(clientId, roomId);
        clients.Add(clientId, client);
        return true;
    }

    public IClient? Get(Guid clientId)
    {
        if (clients.TryGetValue(clientId, out var client)) return client;

        Console.WriteLine($"Client {clientId} does not exist");
        return null;
    }
    
    public bool Remove(Guid clientId)
    {
        if (!clients.Remove(clientId, out var client)) return false;
        
        // TODO: Remove client from room

        return false;
    }
    
    private int GeneratePin()
    {
        const int maxAttempts = 50;
        for (var i = 0; i < maxAttempts; i++) {
            var roomPin = new Random().Next(100_000, 999_999);
            if (!roomPins.ContainsKey(roomPin)) return roomPin;
        }
        
        throw new Exception("Failed to generate a unique room pin");
    }
}