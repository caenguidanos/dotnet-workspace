namespace Common_UnitTesting.Domain;

public sealed class ValueOfUnitTest
{
    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var _ = new StringWithValidation("1234");
        });
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenNotThrowArgumentOutOfRangeException()
    {
        Assert.DoesNotThrow(() =>
        {
            var _ = new StringWithValidation("123456");
        });
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenGetValue_ThenReturnsSameValue()
    {
        var valueObject = new StringWithValidation("123499");

        Assert.That(valueObject.GetValue(), Is.EqualTo("123499"));
    }

    private sealed record StringWithValidation : ValueOf<string>
    {
        public StringWithValidation(string value) : base(value)
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