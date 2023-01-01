namespace Ecommerce.Domain.Service;

using Ecommerce.Application.Command;

public interface IProductUpdaterService
{
    public Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}