using CardHub.Domain.Games.Une.Entities;

namespace CardHub.Domain.Games.Une;

public class UneBotService
{
    public bool RegisterGame(UneGame game)
    {
        game.OnUneEvent += Handle;
        Console.WriteLine("BOT Service - Registered");

        return true;
    }

    public bool UnregisterGame(UneGame game)
    {
        game.OnUneEvent -= Handle;
        Console.WriteLine("BOT Service - Unregistered");
 
        return true;
    }

    void Handle(object? sender, UneEventArgs args)
    {
        if (sender is not UneGame game) return;

        var id = game.Id;
        
        Console.WriteLine("BOT Service - Game Event Received, ID: ", id);
    }
}