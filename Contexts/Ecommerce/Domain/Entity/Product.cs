namespace Ecommerce.Domain.Entity;

using System.Globalization;

using Common.Domain;
using Ecommerce.Domain.Error;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class Product : Schema
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required ProductStatusValue Status { get; init; }
    public required int Price { get; init; }

    public ProductPrimitives ToPrimitives()
    {
        return new ProductPrimitives
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Status = Status,
            Price = Price,
            created_at = created_at,
            updated_at = updated_at
        };
    }

    public bool DeepEqual(Product comparer)
    {
        var locale = new CultureInfo("en-US");

        return ShallowEqual(comparer) &&
                 created_at.ToString(locale) == comparer.created_at.ToString(locale) &&
                 updated_at.ToString(locale) == comparer.updated_at.ToString(locale);
    }

    public bool ShallowEqual(Product comparer)
    {
        return Id == comparer.Id &&
                 Title == comparer.Title &&
                 Description == comparer.Description &&
                 Status == comparer.Status &&
                 Price == comparer.Price;
    }

    protected override void Validate()
    {
        // Price
        if (Price is < 100 or > (1_000_000 * 100))
        {
            AddError(new ProductPriceInvalidError());
        }

        // Description
        if (Description.Length is < 5 or > 600)
        {
            AddError(new ProductDescriptionInvalidError());
        }

        //Status
        if (!Enum.IsDefined(Status))
        {
            AddError(new ProductStatusInvalidError());
        }

        //Title
        if (Title.Length is < 5 or > 256)
        {
            AddError(new ProductTitleInvalidError());
        }
    }
}
