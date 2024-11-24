using CardHub.Games.Common;

namespace CardHub.Games.Une.Card;

public enum UneColor
{
    Blue,
    Green,
    Red,
    Yellow,
    Black
}

public record UneCard : ICard
{
    public int Id { get; init; }
    public UneColor Color { get; }
    public UneAction? Action { get; }
    public int? Number { get; }
    public int DrawAmount { get; }
    public int Points { get; }

    public UneCard(int Id, UneColor Color, UneAction? Action = default, int? Number = null, int DrawAmount = 0)
    {
        this.Id = Id;
        this.Color = Color;
        this.Action = Action;
        this.Number = Number;
        this.DrawAmount = DrawAmount;
        Points = CalculatePoints();
    }

    public bool IsDrawable => DrawAmount > 0;

    public override string ToString()
    {
        var color = Color == UneColor.Black ? "Wild" : Color.ToString();
        
        var action = Action != null ? Action.Name : string.Empty;
        if (action.Length > 0) action += " ";
        
        var symbol = IsDrawable ? "+" : "";
        
        var quantifier = IsDrawable ? DrawAmount.ToString() : Number.ToString() ?? string.Empty;
        if (quantifier.Length > 0) quantifier += " ";

        var cardName = $"[{Id}] {color} {action}{symbol}{quantifier}({Points}pts)";
        return cardName;
    }

    public int CompareDrawable(UneCard other) => DrawAmount.CompareTo(other.DrawAmount);

    private int CalculatePoints()
    {
        if (Number != null) return Number.Value;
        return Color != UneColor.Black ? 20 : 50;
    }
}