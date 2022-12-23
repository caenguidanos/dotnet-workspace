namespace Ecommerce.Domain.ValueObject;

using Common.Domain;
using Ecommerce.Domain.Exception;
using Ecommerce.Domain.Model;

public sealed record ProductId : Primitive<Guid>
{
    public ProductId(Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
    }

    public override Guid Validate()
    {
        if (Value == Guid.Empty)
        {
            throw new ProductIdInvalidException();
        }

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
        const int min = 100;
        const int max = 1_000_000 * 100;

        if (Value is < min or > max)
        {
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
        const int minLength = 5;
        const int maxLength = 600;

        if (Value.Length is < minLength or > maxLength)
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
        const int minLength = 5;
        const int maxLength = 256;

        if (Value.Length is < minLength or > maxLength)
        {
            throw new ProductTitleInvalidException();
        }

        return Value;
    }
}