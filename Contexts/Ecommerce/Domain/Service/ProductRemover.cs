namespace Ecommerce.Domain.Service;

using Common.Domain;

public interface IProductRemoverService
{
    Task<Result> RemoveProduct(Guid id, CancellationToken cancellationToken);
}
