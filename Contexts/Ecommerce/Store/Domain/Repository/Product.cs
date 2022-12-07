namespace Ecommerce.Store.Domain.Repository;

using Ecommerce.Store.Domain.Entity;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken);
    Task<Product> GetById(Guid id, CancellationToken cancellationToken);
    Task Save(Product product, CancellationToken cancellationToken);
    Task DeleteById(Guid id, CancellationToken cancellationToken);
    Task Update(Product product, CancellationToken cancellationToken);
}
