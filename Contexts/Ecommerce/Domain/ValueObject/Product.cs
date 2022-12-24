namespace Ecommerce.Domain;

public sealed record ProductId : ValueOf<Guid>
{
    public ProductId(Guid? id = null) : base(id ?? Guid.NewGuid())
    {
    }

    protected override void TryValidation()
    {
        if (Value == Guid.Empty)
        {
            throw new ProductIdInvalidException();
        }
    }
}

public sealed record ProductPrice : ValueOf<int>
{
    public ProductPrice(int value) : base(value)
    {
    }

    protected override void TryValidation()
    {
        const int min = 100;
        const int max = 1_000_000 * 100;

        if (Value is < min or > max)
        {
            throw new ProductPriceInvalidException();
        }
    }
}

public sealed record ProductDescription : ValueOf<string>
{
    public ProductDescription(string value) : base(value)
    {
    }

    protected override void TryValidation()
    {
        const int minLength = 5;
        const int maxLength = 600;

        if (Value.Length is < minLength or > maxLength)
        {
            throw new ProductDescriptionInvalidException();
        }
    }
}

public sealed record ProductStatus : ValueOf<ProductStatusValue>
{
    public ProductStatus(ProductStatusValue value) : base(value)
    {
    }

    protected override void TryValidation()
    {
        if (!Enum.IsDefined(Value))
        {
            throw new ProductStatusInvalidException();
        }
    }
}

public sealed record ProductTitle : ValueOf<string>
{
    public ProductTitle(string value) : base(value)
    {
    }

    protected override void TryValidation()
    {
        const int minLength = 5;
        const int maxLength = 256;

        if (Value.Length is < minLength or > maxLength)
        {
            throw new ProductTitleInvalidException();
        }
    }
}