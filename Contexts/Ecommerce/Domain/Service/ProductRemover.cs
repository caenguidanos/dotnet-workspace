namespace Ecommerce.Domain;

using Common.Domain;

using OneOf;

public interface IProductRemoverService
{
    Task<OneOf<byte, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}