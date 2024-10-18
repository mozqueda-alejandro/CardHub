namespace CardHub.Games.Common;

public interface IBaseHub
{
    Task StartGame();
    Task EndGame();
    Task Pause();
    Task Resume();
    Task KickPlayer();
    Task RestartGame();
}