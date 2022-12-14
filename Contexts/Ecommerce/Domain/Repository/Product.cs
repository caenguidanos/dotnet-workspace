namespace Ecommerce.Domain.Repository;

using Common.Domain;

using Ecommerce.Domain.Entity;

public interface IProductRepository
{
    Task<Result<IEnumerable<Product>>> Get(CancellationToken cancellationToken);
    Task<Result<Product>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<bool>> Save(Product product, CancellationToken cancellationToken);
    Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken);
    Task<Result<bool>> Update(Product product, CancellationToken cancellationToken);
}
