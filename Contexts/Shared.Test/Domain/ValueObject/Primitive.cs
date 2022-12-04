// <copyright file="Primitive.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Shared.Test.Domain.ValueObject;

using Shared.Domain.ValueObject;

public class Primitive
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenThrowArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new StringPrimitiveWithValidation("1234"));
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenCreateInstance_ThenNotThrowArgumentOutOfRangeException()
    {
        Assert.DoesNotThrow(() => new StringPrimitiveWithValidation("123456"));
    }

    [Test]
    public void GivenStringPrimitiveWithValidation_WhenGetValue_ThenReturnsSameValue()
    {
        var primitive = new StringPrimitiveWithValidation("123499");

        Assert.That<string>(primitive.GetValue(), Is.EqualTo("123499"));
    }

    private class StringPrimitiveWithValidation : ValueObject<string>
    {
        public StringPrimitiveWithValidation(string value)
            : base(value)
        {
        }

        public override string Validate(string value)
        {
            var mAX_LENGTH = 10;
            var mIN_LENGTH = 5;

            if (value.Length < mIN_LENGTH || value.Length > mAX_LENGTH)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }
    }
}
