namespace CardHub.Games.Une.Card;

public class UneAction
{
    public string Name { get; init; }
    public bool? IsWild { get; init; }
    public bool IsDrawable { get; }

    private UneAction(string name, bool isDrawable, bool? isWild = null)
    {
        Name = name;
        IsDrawable = isDrawable;
        IsWild = isWild;
    }

    // @formatter:off
    public static readonly UneAction Block         = new(nameof(Block),        false, true);
    public static readonly UneAction Bomb          = new(nameof(Bomb),         false, true);
    public static readonly UneAction ColorRoulette = new(nameof(ColorRoulette),false, true);
    public static readonly UneAction DiscardColor  = new(nameof(DiscardColor), false, false);
    public static readonly UneAction Draw          = new(nameof(Draw),         true);
    public static readonly UneAction Eye           = new(nameof(Eye),          false, true);
    public static readonly UneAction None          = new(nameof(None),         false);
    public static readonly UneAction Reverse       = new(nameof(Reverse),      false, false);
    public static readonly UneAction ReverseDraw   = new(nameof(ReverseDraw),  true,  true);
    public static readonly UneAction Skip          = new(nameof(Skip),         false, false);
    public static readonly UneAction SkipAll       = new(nameof(SkipAll),      false, false);
    public static readonly UneAction Snap          = new(nameof(Snap),         false, true);
    public static readonly UneAction StealTurn     = new(nameof(StealTurn),    false, true);
    public static readonly UneAction Swap          = new(nameof(Swap),         false, false);
    public static readonly UneAction Target        = new(nameof(Target),       true,  false);
    // @formatter:on
}