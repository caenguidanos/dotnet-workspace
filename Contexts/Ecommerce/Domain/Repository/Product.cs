namespace Contexts.Ecommerce.Domain.Repository;

using Contexts.Ecommerce.Domain.Entity;

public interface IProductRepository
{
    Task<IEnumerable<Product>> Get(CancellationToken cancellationToken);
    Task<IEnumerable<ProductEvent>> GetEvents(CancellationToken cancellationToken);
    Task<Product> GetById(Guid id, CancellationToken cancellationToken);
    Task Save(Product product, CancellationToken cancellationToken);
    Task SaveEvent(ProductEvent ev, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
    Task Update(Product product, CancellationToken cancellationToken);
}
