namespace CardHub.Games.Une.Entities;

public enum StackRule
{
    Equal,
    EqualOrGreater,
    None
}

public class UneSettings
{
    public bool DrawUntil { get; set; }
    public bool ForcePlay { get; set; }
    public bool JumpIn { get; set; }
    public bool Mercy { get; set; }
    public bool SevensSwap { get; set; }
    public bool ZerosPass { get; set; }
    public StackRule StackRule { get; set; }
}