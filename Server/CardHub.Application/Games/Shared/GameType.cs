namespace CardHub.Application.Games.Shared;

public class GameT
{
    public string Name { get; init; }
    public string Url { get; init; }

    private GameT(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public static GameT Une = new GameT("Une", "une");
}

public enum GameType
{
    Une,
    Tens
}