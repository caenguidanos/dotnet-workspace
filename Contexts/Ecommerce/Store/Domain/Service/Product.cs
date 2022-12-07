namespace Ecommerce.Store.Domain.Service;

using Ecommerce.Store.Infrastructure.DataTransfer;

public interface IProductService
{
    Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
    Task DeleteProductById(Guid id, CancellationToken cancellationToken);
    Task UpdateProductById(Guid id, PartialProduct partialProduct, CancellationToken cancellationToken);
}
