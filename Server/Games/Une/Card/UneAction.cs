namespace CardHub.Games.Une.Card;

public class UneAction
{
    public string Name { get; }
    public bool IsDrawable { get; }
    public bool IsWild { get; }
    public bool IsStackable { get; }

    private UneAction(string name, bool isDrawable, bool isWild, bool isStackable = false)
    {
        Name = name;
        IsDrawable = isDrawable;
        IsWild = isWild;
        
        if (!IsDrawable && IsStackable) throw new ArgumentException("Non-drawable cards cannot stack draw amounts");
        
        IsStackable = isStackable;
    }

    // @formatter:off
    public static readonly UneAction Block         = new(nameof(Block),        false, true);
    public static readonly UneAction Bomb          = new(nameof(Bomb),         false, true);
    public static readonly UneAction ColorRoulette = new(nameof(ColorRoulette),false, true); //nm
    public static readonly UneAction DiscardColor  = new(nameof(DiscardColor), false, false); //nm
    public static readonly UneAction Eye           = new(nameof(Eye),          false, true);
    public static readonly UneAction JumpIn        = new(nameof(JumpIn),       false, true);
    public static readonly UneAction Reverse       = new(nameof(Reverse),      false, false);
    public static readonly UneAction ReverseDraw   = new(nameof(ReverseDraw),  true,  true,  true); //nm
    public static readonly UneAction Shell         = new(nameof(Shell),        true,  false);
    public static readonly UneAction Skip          = new(nameof(Skip),         false, false);
    public static readonly UneAction SkipAll       = new(nameof(SkipAll),      false, false); //nm
    public static readonly UneAction Snap          = new(nameof(Snap),         false, true);
    public static readonly UneAction Steal         = new(nameof(Steal),        false, true);
    public static readonly UneAction Swap          = new(nameof(Swap),         false, false);
    public static readonly UneAction Target        = new(nameof(Target),       true,  false, true);
    // @formatter:on

    public override string ToString() => Name;
}