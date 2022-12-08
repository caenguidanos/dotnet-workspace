namespace Common.Domain;

public abstract class Primitive<K>
{
    private readonly K value;

    public Primitive(K value)
    {
        this.value = Validate(value);
    }

    protected abstract K Validate(K value);

    public K GetValue()
    {
        return value;
    }
}
