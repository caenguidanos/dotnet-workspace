namespace Ecommerce.Domain.Entity;

using Common.Domain;
using Ecommerce.Domain.Exception;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.DataTransfer;

public sealed record Product : Schema<ProductPrimitives>
{
    public required ProductId Id { private get; init; }
    public required ProductTitle Title { private get; init; }
    public required ProductDescription Description { private get; init; }
    public required ProductStatus Status { private get; init; }
    public required ProductPrice Price { private get; init; }

    public override ProductPrimitives ToPrimitives()
    {
        return new ProductPrimitives
        {
            Id = Id.GetValue(),
            Title = Title.GetValue(),
            Description = Description.GetValue(),
            Status = Status.GetValue(),
            Price = Price.GetValue(),
            CreatedAt = CreatedAd,
            UpdatedAt = UpdatedAt,
        };
    }

    public Result<ResultUnit, ProblemDetailsException> CheckIntegrity()
    {
        try
        {
            Id.Validate();
            Title.Validate();
            Description.Validate();
            Status.Validate();
            Price.Validate();

            return new Result<ResultUnit, ProblemDetailsException>();
        }
        catch (ProblemDetailsException ex)
        {
            return new Result<ResultUnit, ProblemDetailsException>(ex);
        }
    }
}