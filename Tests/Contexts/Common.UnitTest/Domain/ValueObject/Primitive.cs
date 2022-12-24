namespace Common.UnitTest.Domain;

using Common.Domain;

public sealed class PrimitiveTestUnitTest
{
    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var _ = new StringValueOfWithValidation("1234");
        });
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenNotThrowArgumentOutOfRangeException()
    {
        Assert.DoesNotThrow(() =>
        {
            var _ = new StringValueOfWithValidation("123456");
        });
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenGetValue_ThenReturnsSameValue()
    {
        var valueObject = new StringValueOfWithValidation("123499");

        Assert.That(valueObject.GetValue(), Is.EqualTo("123499"));
    }

    private sealed record StringValueOfWithValidation : ValueOf<string>
    {
        public StringValueOfWithValidation(string value) : base(value)
        {
        }

        protected override void TryValidation()
        {
            const int maxLength = 10;
            const int minLength = 5;

            if (Value.Length is < minLength or > maxLength)
            {
                throw new ArgumentOutOfRangeException(Value);
            }
        }
    }
}