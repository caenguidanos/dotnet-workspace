namespace Ecommerce.Domain.Service;

using Common.Domain;

public interface IProductRemoverService
{
    Task<Result<bool>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}
