namespace api.Contexts.Shared.Domain
{
    abstract public class ValueObject<Primitive>
    {
        private readonly Primitive _value;

        public ValueObject(Primitive value)
        {
            _value = validate(value);
        }

        public abstract Primitive validate(Primitive value);

        public Primitive GetValue() => _value;
    }
}