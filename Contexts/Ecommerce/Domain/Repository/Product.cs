namespace Ecommerce.Domain.Repository;

using Common.Domain;

using Ecommerce.Domain.Entity;

public interface IProductRepository
{
    Task<Result<IEnumerable<Product>>> Get(CancellationToken cancellationToken);
    Task<Result<Product>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result> Save(Product product, CancellationToken cancellationToken);
    Task<Result> Delete(Guid id, CancellationToken cancellationToken);
    Task<Result> Update(Product product, CancellationToken cancellationToken);
}
