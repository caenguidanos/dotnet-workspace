namespace Ecommerce.Domain.Service;

using Common.Domain;
using Ecommerce.Application.Command;
using OneOf;

public interface IProductUpdaterService
{
    Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(
        Guid id,
        UpdateProductCommand command,
        CancellationToken cancellationToken);
}