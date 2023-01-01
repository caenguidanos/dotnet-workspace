namespace Ecommerce.Domain.Service;

public interface IProductRemoverService
{
    public Task<OneOf<byte, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}