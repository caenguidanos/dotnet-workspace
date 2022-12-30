namespace Ecommerce.Domain;

public interface IProductUpdaterService
{
    public Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}