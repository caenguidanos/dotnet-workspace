namespace Ecommerce.Domain.Repository;

using Common.Domain;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Error;

public interface IProductRepository
{
    Task<Result<IEnumerable<Product>, ProductError>> Get(CancellationToken cancellationToken);
    Task<Result<Product, ProductError>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<byte, ProductError>> Save(Product product, CancellationToken cancellationToken);
    Task<Result<byte, ProductError>> Delete(Guid id, CancellationToken cancellationToken);
    Task<Result<byte, ProductError>> Update(Product product, CancellationToken cancellationToken);
}
