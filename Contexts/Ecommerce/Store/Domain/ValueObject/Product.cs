namespace Ecommerce.Store.Domain.ValueObject;

using System.Globalization;
using Ecommerce.Store.Domain.Model;

public class ProductId : ValueObject<Guid>
{
    public ProductId(Guid value)
        : base(value)
    {
    }

    public override Guid Validate(Guid value)
    {
        return value;
    }
}

public class ProductPrice : ValueObject<int>
{
    public ProductPrice(int value)
        : base(value)
    {
    }

    public override int Validate(int value)
    {
        int min = 100;
        int max = 100_000;

        if (value < min || value > max)
        {
            var locale = new CultureInfo("en-US");

            throw new ProductPriceInvalidException(value.ToString(locale));
        }

        return value;
    }
}

public class ProductDescription : ValueObject<string>
{
    public ProductDescription(string value)
        : base(value)
    {
    }

    public override string Validate(string value)
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

public class ProductStatus : ValueObject<ProductStatusValue>
{
    public ProductStatus(ProductStatusValue value)
        : base(value)
    {
    }

    public override ProductStatusValue Validate(ProductStatusValue value)
    {
        if (!Enum.IsDefined(value))
        {
            throw new ProductStatusInvalidException(value.ToString());
        }

        return value;
    }
}

public class ProductTitle : ValueObject<string>
{
    public ProductTitle(string value)
        : base(value)
    {
    }

    public override string Validate(string value)
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
