namespace CardHub.Games.Une.Card;

public class UneCardBuilder
{
    private UneAction _action;
    private List<UneColor> _colors = [];
    private List<int> _numbers = [];
    private List<int> _drawValues = [];
    private int _startId;

    public UneCardBuilder(int id, UneAction? action = null)
    {
        _startId = id;
        _action = action ?? UneAction.None;

        var isWild = _action.IsWild;
        if (isWild.HasValue && isWild.Value) _colors.Add(UneColor.Black);
    }

    #region ColorSet

    public UneCardBuilder SetStandardColors()
    {
        List<UneColor> standardColors = [UneColor.Blue, UneColor.Green, UneColor.Red, UneColor.Yellow];
        return SetColors(standardColors);
    }
    public UneCardBuilder SetWildColor()
    {
        var onlyBlackAdded = _colors.All(color => color == UneColor.Black);
        
        var standardColorAdded = _colors.Any(c => c != UneColor.Black);
        if (standardColorAdded) throw new InvalidOperationException("Colors already added");
        
        return SetColor(UneColor.Black);
    }
    public UneCardBuilder SetColor(UneColor color) => SetColors([color]);
    public UneCardBuilder SetColors(List<UneColor> colors)
    {
        if (_colors.Count != 0) throw new InvalidOperationException("Action is wild, color must be black");
        if (_colors.Count > 0 && _colors.Contains(UneColor.Black))
            throw new InvalidOperationException("Wild/nonwild cards cannot be created together");

        _colors.AddRange(colors);
        return this;
    }

    #endregion

    #region NumberSet

    public UneCardBuilder SetNumber(int number) => SetNumbers([number]);
    public UneCardBuilder SetNumbers(int start, int end, List<int>? excludedNumbers = null)
    {
        if (end < start) throw new ArgumentException("End value must be greater than or equal to start value.");

        var count = end - start + 1;
        var numbers = Enumerable.Range(start, count).ToList();

        if (excludedNumbers != null)
        {
            numbers = numbers.Except(excludedNumbers).ToList();
        }

        return SetNumbers(numbers);
    }
    public UneCardBuilder SetNumbers(List<int> numbers)
    {
        if (_drawValues.Count != 0) throw new InvalidOperationException("Draw value(s) already initialized");

        var isWild = _action.IsWild;
        if (isWild.HasValue && isWild.Value) throw new InvalidOperationException("Action cards cannot have a number");

        _numbers.AddRange(numbers);
        return this;
    }

    #endregion

    #region DrawValueSet

    public UneCardBuilder SetDrawValue(int drawValue) => SetDrawValues([drawValue]);

    public UneCardBuilder SetDrawValues(List<int> drawValues)
    {
        if (_numbers.Count != 0) throw new InvalidOperationException("Number(s) already initialized");

        _drawValues.AddRange(drawValues);
        return this;
    }

    #endregion

    public UneCard Build(out int nextId)
    {
        Validate();

        int? value = null;
        if (_numbers.Count > 0) value = _numbers[0];
        else if (_drawValues.Count > 0) value = _drawValues[0];

        var color = _colors[0];
        var toBuild = new UneCard(_startId++, _action, color, value);
        nextId = _startId;
        return toBuild;
    }

    public List<UneCard> BuildRange(out int nextId)
    {
        Validate();

        var values = new List<int>();
        if (_numbers.Count > 0) values = _numbers;
        else if (_drawValues.Count > 0) values = _drawValues;

        var toBuild = new List<UneCard>();
        foreach (var color in _colors)
        {
            if (values.Count > 0)
            {
                var toAdd = values.Select(value => new UneCard(_startId++, _action, color, value));
                toBuild.AddRange(toAdd);
            }
            else
            {
                toBuild.Add(new UneCard(_startId++, _action, color));
            }
        }

        nextId = _startId;
        return toBuild;
    }

    private void Validate()
    {
        if (_colors.Count == 0)
            throw new InvalidOperationException("Color must be provided");
        if (_drawValues.Count == 0 && _action.IsDrawable)
            throw new InvalidOperationException("Draw value must be provided for drawable action");
        if (_drawValues.Count > 0 && _numbers.Count > 0)
            throw new InvalidOperationException("Numbers and draw values cannot be both set");
        if (_action == UneAction.None && _numbers.Count == 0)
            throw new InvalidOperationException("Number(s) must be provided for standard card");
    }
}