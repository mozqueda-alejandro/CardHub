using System.Timers;
using Timer = System.Timers.Timer;

namespace CardHub.Domain.Games.Shared;

public class GameTimer
{
    private readonly Timer _timer;
    private readonly Action _onTimeElapsed;

    public GameTimer(double interval, Action onTimeElapsed)
    {
        _onTimeElapsed = onTimeElapsed;
        _timer = new Timer(interval)
        {
            AutoReset = false
        };
        _timer.Elapsed += TimerElapsed;
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _onTimeElapsed.Invoke();
    }

    public void Start()
    {
        _timer.Stop();
        _timer.Start();
    }

    public void Restart()
    {
        _timer.Stop();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
