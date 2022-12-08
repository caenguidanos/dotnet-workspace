namespace Contexts.Common.Domain;

public abstract class Primitive<TValue>
{
    private readonly TValue value;

    public Primitive(TValue value)
    {
        this.value = Validate(value);
    }

    protected abstract TValue Validate(TValue value);

    public TValue GetValue()
    {
        return value;
    }
}
