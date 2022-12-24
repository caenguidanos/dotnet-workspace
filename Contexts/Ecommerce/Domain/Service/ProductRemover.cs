namespace Ecommerce.Domain;

public interface IProductRemoverService
{
    Task<OneOf<byte, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}