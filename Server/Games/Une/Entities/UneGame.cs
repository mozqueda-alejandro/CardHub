using CardHub.Games.Common;
using CardHub.Games.Une.Card;

namespace CardHub.Games.Une.Entities;

public class UneGame
{
    private Deck<UneCard> _deck;
    private UneOrder<UnePlayer> _order;
    private int _toDraw;

    public UneGame(UnePlayer[] players)
    {
        _deck = new Deck<UneCard>();
        _order = new UneOrder<UnePlayer>(players);
        _toDraw = 0;

        var id = 0;

        var redReverse = new UneCardBuilder(id, UneAction.Reverse)
            .SetColor(UneColor.Red)
            .Build(out id);
        _deck.Add(redReverse);
        Console.WriteLine(redReverse);

        var allStandards = new UneCardBuilder(id)
            .SetStandardColors()
            .SetNumbers(1, 10)
            .BuildRange(out id);
        _deck.AddRange(allStandards);
        allStandards.ForEach(Console.WriteLine);

        var block = new UneCardBuilder(id, UneAction.Block)
            .BuildRange(out id);
        _deck.AddRange(block);
        block.ForEach(Console.WriteLine);
        
        var wildR4 = new UneCardBuilder(id, UneAction.ReverseDraw)
            .SetDrawValue(4)
            .Build(out id);
        _deck.Add(wildR4);
        Console.WriteLine(wildR4);
        
        var wild4 = new UneCardBuilder(id, UneAction.Draw)
            .SetDrawValue(4)
            .SetStandardColors()
            .BuildRange(out id);
        _deck.AddRange(wild4);
        wild4.ForEach(Console.WriteLine);
        
        
    }

    public async Task StartGame()
    {
        _order.Shuffle();
        _deck.Shuffle();

        foreach (var player in _order)
        {
            var drawnCards = _deck.Draw(7);
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
    }

    public async Task TurnExpired()
    {
        var toDraw = _deck.Draw(2);
        _order.Current.AddRange(toDraw);
        _order.Iterate();
    }

    public async Task EndGame(string winner = "")
    {
        Console.WriteLine($"{winner} has won!");
    }

    public async Task<bool> PlayCard(string playerName, int cardId)
    {
        var player = _order.Find(playerName);
        if (player == null) return false;
        if (!player.Equals(_order.Current)) return false;

        var card = player.Get(cardId);
        if (card == null) return false;
        if (!await IsPlayable(card)) return false;

        player.Play(cardId);
        _deck.Discard(card);

        if (player.Count != 0)
        {
            _order.Iterate();
        }
        else
        {
            await EndGame(playerName);
        }

        return true;
    }

    public async Task<UneStatePlayer> GetStatePlayer(string name)
    {
        var player = _order.Find(name);
        if (player == null) throw new KeyNotFoundException(name);

        var state = new UneStatePlayer { Hand = player.Hand.ToList() };

        return state;
    }

    private async Task<bool> IsPlayable(UneCard card)
    {
        // var lastPlayed = _deck.Current;
        // if (lastPlayed is null) throw new NullReferenceException("No cards have been played");
        //
        // var noColorMatch = card.Color != lastPlayed.Color
        //                    && card.Color != UneColor.Black; // Exclude wild cards in color match
        // var noNumberMatch = card.Number != lastPlayed.Number;
        // var noSpecialMatch = card.SpecialType != lastPlayed.SpecialType;
        // var notPlayableWild = card.Color != UneColor.Black && card.SpecialType == lastPlayed.SpecialType;
        //
        // if (noColorMatch && noNumberMatch && noSpecialMatch && notPlayableWild) return false;
        // return true;

        return false;
    }
}