namespace Common.Domain.ValueObject;

public abstract class ValueObject<Primitive>
{
    private readonly Primitive value;

    public ValueObject(Primitive value)
    {
        this.value = Validate(value);
    }

    public abstract Primitive Validate(Primitive value);

    public Primitive GetValue()
    {
        return value;
    }
}
