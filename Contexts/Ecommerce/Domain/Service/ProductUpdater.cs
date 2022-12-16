namespace Ecommerce.Domain.Service;

using Common.Domain;

using Ecommerce.Application.Command;
using Ecommerce.Domain.Error;

public interface IProductUpdaterService
{
    Task<Result<byte, ProductError>> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}
