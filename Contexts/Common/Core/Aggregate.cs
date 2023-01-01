namespace Common.Core;

public interface IAggregateRoot<out TBase, TBasePrimitives>
{
    public TBasePrimitives ToPrimitives();
    public static virtual TBase FromPrimitives(TBasePrimitives primitives) => throw new NotImplementedException();
}