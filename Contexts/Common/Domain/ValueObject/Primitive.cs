namespace Common.Domain;

public record Primitive<TValue>
{
    protected TValue Value { get; }

    protected Primitive(TValue value)
    {
        Value = value;
    }

    public virtual TValue Validate() => throw new NotImplementedException();

    public TValue GetValue() => Value;
}