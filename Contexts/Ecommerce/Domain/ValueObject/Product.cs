namespace Ecommerce.Domain.ValueObject;

using System.Globalization;
using Common.Domain;
using Ecommerce.Domain.Error;
using Ecommerce.Domain.Model;

public sealed record ProductId : Primitive<Guid>
{
    public ProductId(Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
    }

    public override Guid Validate()
    {
        return Value;
    }
}

public sealed record ProductPrice : Primitive<int>
{
    public ProductPrice(int value)
        : base(value)
    {
    }

    public override int Validate()
    {
        int min = 100;
        int max = 1_000_000 * 100;

        if (Value < min || Value > max)
        {
            _ = new CultureInfo("en-US");

            throw new ProductPriceInvalidException();
        }

        return Value;
    }
}

public sealed record ProductDescription : Primitive<string>
{
    public ProductDescription(string value)
        : base(value)
    {
    }

    public override string Validate()
    {
        int minLength = 5;
        int maxLength = 600;

        if (Value.Length < minLength || Value.Length > maxLength)
        {
            throw new ProductDescriptionInvalidException();
        }

        return Value;
    }
}

public sealed record ProductStatus : Primitive<ProductStatusValue>
{
    public ProductStatus(ProductStatusValue value)
        : base(value)
    {
    }

    public override ProductStatusValue Validate()
    {
        if (!Enum.IsDefined(Value))
        {
            throw new ProductStatusInvalidException();
        }

        return Value;
    }
}

public sealed record ProductTitle : Primitive<string>
{
    public ProductTitle(string value)
        : base(value)
    {
    }

    public override string Validate()
    {
        int minLength = 5;
        int maxLength = 256;

        if (Value.Length < minLength || Value.Length > maxLength)
        {
            throw new ProductTitleInvalidException();
        }

        return Value;
    }
}
