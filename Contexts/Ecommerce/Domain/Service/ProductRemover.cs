namespace Ecommerce.Domain.Service;

public interface IProductRemoverService
{
    Task RemoveProduct(Guid id, CancellationToken cancellationToken);
}
