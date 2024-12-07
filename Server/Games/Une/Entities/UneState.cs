namespace CardHub.Games.Une.Entities;

public abstract class UneState
{
    protected UneGame _game;

    public void SetState(UneGame game)
    {
        _game = game;
    }

    public abstract bool PickColor();
    public abstract bool PickPlayer();
    public abstract bool PlayCard();
    public abstract bool SayUne();
}

public class UneStartState : UneState
{
    public override bool PickColor() => false;
    public override bool PickPlayer() => false;
    public override bool PlayCard() => false;
    public override bool SayUne() => false;
}

public class UneEndState : UneState
{
    public override bool PickColor() => false;
    public override bool PickPlayer() => false;
    public override bool PlayCard() => false;
    public override bool SayUne() => false;
}

// public class UnePlayableState : UneState
// {
//     public override bool PickColor()
//     {
//         
//     }
//
//     public override bool PickPlayer()
//     {
//         
//     }
//     
//     public override bool PlayCard()
//     {
//         
//     }
//
//     public override bool SayUne()
//     {
//         
//     }
// }