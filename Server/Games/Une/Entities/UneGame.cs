using CardHub.Games.Common;
using CardHub.Games.Une.Card;

namespace CardHub.Games.Une.Entities;

public enum GameState
{
    Init,
    End,
    PickColor,
    PickPlayer,
    Playable,
    Start
}

public class UneGame : IGame
{
    private readonly IDictionary<int, UneCard> _cardSet;

    private UneSettings _settings;
    private UneDeckFactory _deckFactory;
    private Deck<UneCard> _deck;
    private Order<UnePlayer> _order;
    private int _toDraw;

    private GameState _state = GameState.Init;

    // private UneState _state = new UneStartState();
    private bool _sayUneResolved = false;

    public int MaxPlayers { get; set; } = 8;

    public UneGame(IDictionary<int, UneCard> cardSet, UneDeckFactory deckFactory)
    {
        _cardSet = cardSet;
        _deckFactory = deckFactory;
        _deck = new Deck<UneCard>();
    }

    public void Init(List<UnePlayer> players, UneSettings settings)
    {
        if (!IsState(GameState.Init)) return;

        _settings = settings;
        _order = new Order<UnePlayer>(players);

        _state = GameState.Start;
    }

    public async Task Start()
    {
        if (!IsState(GameState.Start)) return;

        _deck.AddRange(_deckFactory.Create());
        _order.Shuffle();
        _deck.Shuffle();

        foreach (var player in _order)
        {
            var drawnCards = _deck.Draw(4);
            player.AddRange(drawnCards);
        }

        var counter = 0;
        while (counter++ < 10)
        {
            var topCard = _deck.Draw();
            if (topCard is null) throw new ArgumentNullException(nameof(topCard));

            _deck.Discard(topCard);
            if (topCard.Color == UneColor.Black) continue;

            break;
        }

        _state = GameState.Playable;
    }

    public async Task TurnExpired()
    {
        if (!IsState(GameState.Playable, GameState.PickColor, GameState.PickPlayer)) return;

        var toDraw = _deck.Draw(2);
        _order.Current.AddRange(toDraw);
        _order.Iterate();

        _state = GameState.Playable;
    }

    public async Task End(string winner = "")
    {
        Console.WriteLine($"{winner} has won!");
    }

    public async Task<bool> PlayCard(string playerName, int cardId)
    {
        if (!IsState(GameState.Playable)) return false;

        // TODO: Jump in functionality
        var player = _order.Find(playerName);
        if (player == null) return false;
        if (!player.Equals(_order.Current)) return false;

        var card = player.Get(cardId);
        if (card == null) return false;
        if (!await IsPlayable(card)) return false;

        // TODO: Stacking functionality
        if (card.IsDrawable)
        {
            _toDraw += card.DrawAmount;
        }
        else if (_toDraw > 0)
        {
            return false;
        }

        player.Play(cardId);
        _deck.Discard(card);

        if (player.Count == 0)
        {
            _state = GameState.End;
            await End(playerName);
            return true;
        }

        _order.Iterate();
        return true;
    }

    public async Task SayUne(string name)
    {
        if (!IsState(GameState.Playable, GameState.PickColor, GameState.PickPlayer)) return;

        if (_sayUneResolved) return;

        var player = _order.Find(name);
        if (player == null) return;

        var lastPlayer = _order.GetOffset(-1);
        if (player.Equals(lastPlayer)) return; // TODO: Add feedback for last player and/or SayUne player

        var toDraw = _deck.Draw(2);
        lastPlayer.AddRange(toDraw);
    }

    public async Task<UneStatePlayer> GetStatePlayer(string name)
    {
        var player = _order.Find(name);
        if (player == null) throw new KeyNotFoundException(name);

        var state = new UneStatePlayer { Hand = player.Hand.ToList() };

        return state;
    }

    public async Task<UneStateGB> GetStateGB()
    {
        var playerHands = new Dictionary<string, List<UneCard>>();
        foreach (var player in _order)
        {
            playerHands.Add(player.Name, player.Hand);
        }

        return new UneStateGB
            { CurrentPlayer = _order.Current.Name, DiscardPile = _deck.DiscardPile, PlayerHands = playerHands };
    }

    private async Task<bool> IsPlayable(UneCard card)
    {
        var lastPlayed = _deck.Current;
        if (lastPlayed is null) throw new NullReferenceException("No cards have been played");
        
        var noColorMatch = card.Color != lastPlayed.Color
                           && card.Color != UneColor.Black; // Exclude wild cards in color match
        var noNumberMatch = card.Number != lastPlayed.Number;
        // var noSpecialMatch = card.SpecialType != lastPlayed.SpecialType;
        // var notPlayableWild = card.Color != UneColor.Black && card.SpecialType == lastPlayed.SpecialType;
        
        if (noColorMatch && noNumberMatch) return false;
        // if (noColorMatch && noNumberMatch && noSpecialMatch && notPlayableWild) return false;

        return true;
    }

    private bool IsState(params GameState[] validStates) => validStates.Contains(_state);
}