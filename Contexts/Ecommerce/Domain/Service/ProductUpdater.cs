namespace Ecommerce.Domain;

using Common.Domain;

using Application;

using OneOf;

public interface IProductUpdaterService
{
    Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(
        Guid id,
        UpdateProductCommand command,
        CancellationToken cancellationToken);
}