namespace Ecommerce.Domain.Repository;

using Ecommerce.Domain.Entity;

public interface IProductRepository
{
    Task<IEnumerable<Product>> Get(CancellationToken cancellationToken);
    Task<Product> GetById(Guid id, CancellationToken cancellationToken);
    Task Save(Product product, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
    Task Update(Product product, CancellationToken cancellationToken);
}
