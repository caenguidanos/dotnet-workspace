namespace Ecommerce.Domain.Repository;

using Common.Domain;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Error;

public interface IProductRepository
{
    Task<Result<IEnumerable<Product>, ProductException>> Get(CancellationToken cancellationToken);
    Task<Result<Product, ProductException>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<byte, ProductException>> Save(Product product, CancellationToken cancellationToken);
    Task<Result<byte, ProductException>> Delete(Guid id, CancellationToken cancellationToken);
    Task<Result<byte, ProductException>> Update(Product product, CancellationToken cancellationToken);
}
