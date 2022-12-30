namespace Ecommerce.Domain;

public interface IProductRemoverService
{
    public Task<OneOf<byte, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}