namespace Ecommerce.Domain.Service;

using Common.Domain;
using Ecommerce.Application.Command;

public interface IProductUpdaterService
{
    Task<Result> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}
