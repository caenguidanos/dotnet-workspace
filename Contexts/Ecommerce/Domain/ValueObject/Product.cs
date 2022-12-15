namespace Ecommerce.Domain.ValueObject;

using System.Globalization;

using Common.Domain;

using Ecommerce.Domain.Error;
using Ecommerce.Domain.Model;

public sealed class ProductId : Primitive<Guid>
{
    public ProductId(Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
    }

    protected override Guid Validate(Guid value)
    {
        return value;
    }
}

public sealed class ProductPrice : Primitive<int>
{
    public ProductPrice(int value)
        : base(value)
    {
    }

    protected override int Validate(int value)
    {
        int min = 100;
        int max = 1_000_000 * 100;

        if (value < min || value > max)
        {
            _ = new CultureInfo("en-US");

            throw new ProductPriceInvalidError();
        }

        return value;
    }
}

public sealed class ProductDescription : Primitive<string>
{
    public ProductDescription(string value)
        : base(value)
    {
    }

    protected override string Validate(string value)
    {
        int minLength = 5;
        int maxLength = 600;

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new ProductDescriptionInvalidError();
        }

        return value;
    }
}


public sealed class ProductStatus : Primitive<ProductStatusValue>
{
    public ProductStatus(ProductStatusValue value)
        : base(value)
    {
    }

    protected override ProductStatusValue Validate(ProductStatusValue value)
    {
        if (!Enum.IsDefined(value))
        {
            throw new ProductStatusInvalidError();
        }

        return value;
    }
}

public sealed class ProductTitle : Primitive<string>
{
    public ProductTitle(string value)
        : base(value)
    {
    }

    protected override string Validate(string value)
    {
        int minLength = 5;
        int maxLength = 256;

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new ProductTitleInvalidError();
        }

        return value;
    }
}
