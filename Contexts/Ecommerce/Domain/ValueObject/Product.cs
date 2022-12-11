namespace Ecommerce.Domain.ValueObject;

using System.Globalization;

using Common.Domain;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Model;

public sealed class ProductId : Primitive<Guid>
{
    public ProductId(Guid value)
        : base(value)
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
            var locale = new CultureInfo("en-US");

            throw new ProductPriceInvalidException(value.ToString(locale));
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
            throw new ProductDescriptionInvalidException(value);
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
            throw new ProductStatusInvalidException(value.ToString());
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
            throw new ProductTitleInvalidException(value);
        }

        return value;
    }
}
