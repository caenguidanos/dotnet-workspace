namespace Ecommerce.Domain;

public interface IProductUpdaterService
{
    Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}