namespace Ecommerce.Domain.Repository;

using Ecommerce.Domain.Entity;

public interface IProductRepository
{
    Task<OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>> Get(CancellationToken cancellationToken);
    Task<OneOf<ProductPrimitives, ProblemDetailsException>> GetById(Guid id, CancellationToken cancellationToken);
    Task<OneOf<byte, ProblemDetailsException>> Save(Product product, CancellationToken cancellationToken);
    Task<OneOf<byte, ProblemDetailsException>> Delete(Guid id, CancellationToken cancellationToken);
    Task<OneOf<byte, ProblemDetailsException>> Update(Product product, CancellationToken cancellationToken);
}