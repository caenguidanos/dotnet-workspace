namespace Common.Domain;

public abstract class Primitive<TValue>
{
    private TValue value { get; init; }

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
