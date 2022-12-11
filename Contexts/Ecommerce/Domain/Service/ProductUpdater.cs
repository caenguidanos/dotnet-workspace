namespace Ecommerce.Domain.Service;

using Ecommerce.Application.Command;

public interface IProductUpdaterService
{
    Task UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}
