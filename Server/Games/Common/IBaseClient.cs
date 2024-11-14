namespace CardHub.Games.Common;

public interface IBaseClient
{
    Task ReceiveMessage(Message message);
    Task ReceiveAvatars(IEnumerable<string> avatars);
    // Task Paused();
    // Task Resumed();
    Task Kicked();
    Task Pong();
}