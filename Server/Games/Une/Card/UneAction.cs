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
    public static readonly UneAction Discard       = new(nameof(Discard),      false, false);
    public static readonly UneAction DiscardColor  = new(nameof(DiscardColor), false, false); //nm
    public static readonly UneAction Eye           = new(nameof(Eye),          false, true);
    public static readonly UneAction JumpIn        = new(nameof(JumpIn),       false, true);
    public static readonly UneAction Reverse       = new(nameof(Reverse),      false, false);
    public static readonly UneAction ReverseDraw   = new(nameof(ReverseDraw),  true,  true,  true); //nm
    public static readonly UneAction Shell         = new(nameof(Shell),        true,  false);
    public static readonly UneAction ShuffleOrder  = new(nameof(ShuffleOrder), false, false);
    public static readonly UneAction Skip          = new(nameof(Skip),         false, false);
    public static readonly UneAction SkipAll       = new(nameof(SkipAll),      false, false); //nm
    public static readonly UneAction Snap          = new(nameof(Snap),         false, true);
    public static readonly UneAction Steal         = new(nameof(Steal),        false, true);
    public static readonly UneAction Swap          = new(nameof(Swap),         false, false);
    public static readonly UneAction Target        = new(nameof(Target),       true,  false, true);
    // @formatter:on

    public override string ToString() => Name;
}


/*
Each of 4 colors:
2 of each number
3 skips
2 skip all
3 reverse
2 Draw Two
2 Draw Four
3 Discard All of Color

Wilds:
8 reverse Draw Four
4 Draw Six
4 Draw Ten
8 Color Roulette
*/