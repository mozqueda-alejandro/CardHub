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

public class UneCard : ICard
{
    public int Id { get; init; }
    public int DrawValue { get; init; }
    public int? Number { get; init; }
    public UneColor Color { get; }
    public UneAction Action { get; }
    
    public UneCard(int id, UneAction action, UneColor color, int? value = null)
    {
        Id = id;
        Color = color;
        Action = action;

        if (!value.HasValue) return;

        if (action.IsDrawable)
        {
            DrawValue = value.Value;
        }
        else
        {
            Number = value.Value;
        }
    }
    
    public bool IsDrawable => Action.IsDrawable;

    public override string ToString()
    {
        var colorStr = Color == UneColor.Black ? "Wild" : Color.ToString();
        var isActionNone = Action.Name == UneAction.None.Name;
        
        string cardStr;
        var value = IsDrawable ? DrawValue : Number;
        if (isActionNone) cardStr = $"[{Id}] {colorStr} {value}";
        else cardStr = $"[{Id}] {colorStr} {Action.Name} {value}";
        return cardStr;
    }
}
