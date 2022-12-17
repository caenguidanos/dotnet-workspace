namespace Common.Domain;

public record Primitive<TValue>
{
    protected TValue Value { get; init; }

    protected Primitive(TValue value)
    {
        this.Value = value;
    }

    public virtual TValue Validate()
    {
        throw new NotImplementedException();
    }

    public TValue GetValue()
    {
        return Value;
    }
}
