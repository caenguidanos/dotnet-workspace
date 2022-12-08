namespace Ecommerce.Domain.Service;

using Ecommerce.Infrastructure.DataTransfer;

public interface IProductService
{
    Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
    Task DeleteProduct(Guid id, CancellationToken cancellationToken);
    Task UpdateProduct(Guid id, ProductPrimitivesForUpdateOperation product, CancellationToken cancellationToken);
}
