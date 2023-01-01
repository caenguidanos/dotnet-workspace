namespace Common.Core;

public record ValueOf<T>
{
    protected T Value { get; }

    protected ValueOf(in T value)
    {
        Value = value;

        CheckValueObjectIntegrity();
    }

    protected virtual void TryValidation() => throw new NotImplementedException();

    private void CheckValueObjectIntegrity() => TryValidation();

    public T GetValue() => Value;
}