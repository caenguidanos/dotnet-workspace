namespace Ecommerce.Domain.Repository;

using Common.Domain;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exception;

public interface IProductRepository
{
    Task<Result<IEnumerable<Product>, ProblemDetailsException>> Get(CancellationToken cancellationToken);
    Task<Result<Product, ProblemDetailsException>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<byte, ProblemDetailsException>> Save(Product product, CancellationToken cancellationToken);
    Task<Result<byte, ProblemDetailsException>> Delete(Guid id, CancellationToken cancellationToken);
    Task<Result<byte, ProblemDetailsException>> Update(Product product, CancellationToken cancellationToken);
}
