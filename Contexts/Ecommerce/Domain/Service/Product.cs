namespace Ecommerce.Domain.Service;

using Ecommerce.Application.Command;

public interface IProductService
{
    Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
    Task DeleteProduct(Guid id, CancellationToken cancellationToken);
    Task UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken);
}
