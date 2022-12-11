namespace Ecommerce.Domain.Entity;

using System.Globalization;

using Common.Domain;

using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class Product : Schema
{
    public required ProductId Id { get; init; }
    public required ProductTitle Title { get; init; }
    public required ProductDescription Description { get; init; }
    public required ProductStatus Status { get; init; }
    public required ProductPrice Price { get; init; }

    public ProductPrimitives ToPrimitives()
    {
        return new ProductPrimitives
        {
            Id = Id.GetValue(),
            Title = Title.GetValue(),
            Description = Description.GetValue(),
            Status = Status.GetValue(),
            Price = Price.GetValue(),
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
        return Id.GetValue() == comparer.Id.GetValue() &&
                 Title.GetValue() == comparer.Title.GetValue() &&
                 Description.GetValue() == comparer.Description.GetValue() &&
                 Status.GetValue() == comparer.Status.GetValue() &&
                 Price.GetValue() == comparer.Price.GetValue();
    }
}
