namespace Common_UnitTesting.Domain;

[Category("ValueObject")]
public sealed class ValueOfUnitTest
{
    /// <summary>
    ///     Given a string value object,
    ///         when create instance,
    ///             then throws `ArgumentNotFoundException`.
    /// </summary>
    [Test]
    public void ValueObject_I()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var _ = new StringWithValidation("1234");
        });
    }

    /// <summary>
    ///     Given a string value object,
    ///         when create instance,
    ///             then throws `ArgumentOutOfRangeException`.
    /// </summary>
    [Test]
    public void ValueObject_II()
    {
        Assert.DoesNotThrow(() =>
        {
            var _ = new StringWithValidation("123456");
        });
    }

    /// <summary>
    ///     Given a string value object,
    ///         when create instance and return value,
    ///             then is the same value.
    /// </summary>
    [Test]
    public void ValueObject_III()
    {
        const string value = "123499";

        var valueObject = new StringWithValidation(value);

        Assert.That(valueObject.GetValue(), Is.EqualTo(value));
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