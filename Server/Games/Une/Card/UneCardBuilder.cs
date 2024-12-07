using System.Collections;

namespace CardHub.Games.Une.Card;

public class UneCardBuilder
{
    private int _id;
    private UneAction? _action;
    private HashSet<UneColor> _colors = [];
    private HashSet<int> _numbers = [];
    private HashSet<int> _drawAmounts = [];
    private HashSet<UneCard> _cards;

    private IDictionary<int, UneCard> CardSet;
    private Dictionary<int, int> _cardFrequencies = [];

    public UneCardBuilder(IEqualityComparer<UneCard> comparer, IDictionary<int, UneCard> cardSet)
    {
        _cards = new HashSet<UneCard>(comparer);
        CardSet = cardSet;
    }

    private static List<UneColor> _standardColors = [UneColor.Blue, UneColor.Green, UneColor.Red, UneColor.Yellow];

    public UneCardBuilder Action(UneAction action)
    {
        if (_action != null) throw new InvalidOperationException("Action is already set");

        _action = action;
        return this;
    }

    #region Color

    public UneCardBuilder StandardColors() => Colors(_standardColors);
    public UneCardBuilder WildColor() => Color(UneColor.Black);
    public UneCardBuilder Color(UneColor color) => Colors([color]);
    public UneCardBuilder Colors(List<UneColor> colors)
    {
        colors.ForEach(color => _colors.Add(color));
        return this;
    }

    #endregion

    #region Number

    public UneCardBuilder Number(int number) => Numbers([number]);

    /// <param name="start">An integer number specifying at which position to start.</param>
    /// <param name="stop">An integer number specifying at which position to stop (not included).</param>
    /// <param name="exclude">Optional. A list of integers to be excluded.</param>
    public UneCardBuilder NumberRange(int start, int stop, List<int>? exclude = null)
    {
        if (stop < start) throw new ArgumentException("Stop number must be >= to start number.");

        var count = stop - start;
        var numbers = Enumerable.Range(start, count);

        if (exclude != null)
        {
            numbers = numbers.Except(exclude);
        }

        return Numbers(numbers.ToList());
    }
    public UneCardBuilder Numbers(List<int> numbers)
    {
        numbers.ForEach(number => _numbers.Add(number));
        return this;
    }

    #endregion

    #region DrawAmount

    public UneCardBuilder DrawAmount(int drawAmount) => DrawAmounts([drawAmount]);

    public UneCardBuilder DrawAmounts(List<int> drawAmounts)
    {
        drawAmounts.ForEach(amount => _drawAmounts.Add(amount));
        return this;
    }

    #endregion

    public void AddCards(int frequency = 1)
    {
        ConfigureDefaults();
        Validate();

        List<UneCard> toAdd = [];

        foreach (var color in _colors)
        {
            if (NumbersSet)
            {
                toAdd = _numbers.Select(number =>
                {
                    if (!_cardFrequencies.TryAdd(_id, frequency))
                        throw new InvalidOperationException($"Id {_id} frequency already set");

                    return new UneCard(_id++, color, _action, Number: number);
                }).ToList();
            }
            else if (DrawAmountsSet)
            {
                toAdd = _drawAmounts.Select(amount =>
                {
                    if (!_cardFrequencies.TryAdd(_id, frequency))
                        throw new InvalidOperationException($"Id {_id} frequency already set");

                    return new UneCard(_id++, color, _action, DrawAmount: amount);
                }).ToList();
            }
            else
            {
                toAdd = [new UneCard(_id++, color, _action)];
            }

            toAdd.ForEach(card => _cards.Add(card));
        }

        Reset();
    }

    private void ConfigureDefaults()
    {
        if (ColorsSet) return;
        
        if (_action == null)
        {
            Colors(_standardColors);
            return;
        }

        if (_action.IsWild) WildColor();
        else Colors(_standardColors);
    }

    public List<UneCard> Build()
    {
        var cards = _cards.ToList();
        cards.ForEach(card => CardSet.Add(card.Id, card));
        
        return cards;
    }

    private void Validate()
    {
        if (!ColorsSet)
            throw new InvalidOperationException("Must have Color");

        if (DrawAmountsSet && NumbersSet)
            throw new InvalidOperationException("Can either have Number or DrawAmount set");

        var containsNegative = _drawAmounts.Any(amount => amount < 0);
        if (containsNegative) throw new ArgumentException("DrawAmount cannot be negative");

        if (_action == null) return;

        #region ActionProcessing

        if (_action.IsDrawable != DrawAmountsSet)
        {
            var error = _action.IsDrawable
                ? "Drawable action card must have DrawAmount set"
                : "Non-drawable card has DrawAmount set";
            throw new InvalidOperationException(error);
        }

        if (_action.IsWild && NumbersSet)
            throw new InvalidOperationException("Wild card cannot have a Number");

        var containsNonWild = _colors.Any(color => color != UneColor.Black);
        if (_action.IsWild && containsNonWild)
            throw new ArgumentException("Wild card cannot have standard colors");

        var containsWild = _colors.Contains(UneColor.Black);
        if (!_action.IsWild && containsWild)
            throw new ArgumentException("Non-wild card cannot have Black color");

        #endregion
    }

    private bool ColorsSet => _colors.Count > 0;
    private bool NumbersSet => _numbers.Count > 0;
    private bool DrawAmountsSet => _drawAmounts.Count > 0;

    private void Reset()
    {
        _action = null;
        _colors.Clear();
        _numbers.Clear();
        _drawAmounts.Clear();
    }
}