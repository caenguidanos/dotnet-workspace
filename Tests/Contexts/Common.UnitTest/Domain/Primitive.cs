namespace Common.UnitTest.Domain;

using Common.Domain;

public sealed class PrimitiveTestUnitTest
{
    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenThrowsArgumentOutOfRangeException()
    {
        var primitive = new StringPrimitiveWithValidation("1234");

        Assert.Throws<ArgumentOutOfRangeException>(() => primitive.Validate());
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenNotThrowArgumentOutOfRangeException()
    {
        var primitive = new StringPrimitiveWithValidation("123456");

        Assert.DoesNotThrow(() => primitive.Validate());
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenGetValue_ThenReturnsSameValue()
    {
        var primitive = new StringPrimitiveWithValidation("123499");

        Assert.That(primitive.GetValue(), Is.EqualTo("123499"));
    }

    private sealed record StringPrimitiveWithValidation : Primitive<string>
    {
        public StringPrimitiveWithValidation(string value)
            : base(value)
        {
        }

        public override string Validate()
        {
            int maxLength = 10;
            int minLength = 5;

            if (Value.Length < minLength || Value.Length > maxLength)
            {
                throw new ArgumentOutOfRangeException(Value);
            }

            return Value;
        }
    }
}
