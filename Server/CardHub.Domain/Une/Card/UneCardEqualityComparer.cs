namespace CardHub.Domain.Une.Card;

public class UneCardEqualityComparer : IEqualityComparer<UneCard>
{
    public bool Equals(UneCard? x, UneCard? y)
    {
        if (x is null || y is null) return false;
        
        return x.Color == y.Color &&
               x.Action == y.Action &&
               x.DrawAmount == y.DrawAmount &&
               x.Number == y.Number;
    }
    
    public int GetHashCode(UneCard obj) => HashCode.Combine(obj.Color, obj.Action, obj.DrawAmount, obj.Number);
}