namespace Ecommerce.Domain;

public interface IProductRepository
{
    Task<OneOf<List<Product>, ProblemDetailsException>> Get(CancellationToken cancellationToken);
    Task<OneOf<Product, ProblemDetailsException>> GetById(Guid id, CancellationToken cancellationToken);
    Task<OneOf<byte, ProblemDetailsException>> Save(Product product, CancellationToken cancellationToken);
    Task<OneOf<byte, ProblemDetailsException>> Delete(Guid id, CancellationToken cancellationToken);
    Task<OneOf<byte, ProblemDetailsException>> Update(Product product, CancellationToken cancellationToken);
}