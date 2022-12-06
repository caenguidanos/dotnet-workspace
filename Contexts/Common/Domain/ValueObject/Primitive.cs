namespace Common.Domain.ValueObject;

public abstract class ValueObject<TPrimitive>
{
    private readonly TPrimitive value;

    public ValueObject(TPrimitive value)
    {
        this.value = Validate(value);
    }

    public abstract TPrimitive Validate(TPrimitive value);

    public TPrimitive GetValue()
    {
        return value;
    }
}
