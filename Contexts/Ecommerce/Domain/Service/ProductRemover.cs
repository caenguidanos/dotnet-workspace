namespace Ecommerce.Domain.Service;

using Common.Domain;

using Ecommerce.Infrastructure.DataTransfer;

public interface IProductRemoverService
{
    Task<Result<ProductAck, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken);
}
