namespace CardHub.Domain.Games.Une.Commands;

public class PlayCardCommand : UneCommand
{
    
    
    protected override void Execute()
    {
        Console.WriteLine("Play Card Command");
    }
}