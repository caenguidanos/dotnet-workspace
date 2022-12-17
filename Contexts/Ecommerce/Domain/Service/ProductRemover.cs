namespace Ecommerce.Domain.Service;

using Common.Domain;
using Ecommerce.Domain.Error;

public interface IProductRemoverService
{
    Task<Result<byte, ProductException>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}
