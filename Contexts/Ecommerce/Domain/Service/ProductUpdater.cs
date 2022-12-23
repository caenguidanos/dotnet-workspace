namespace Ecommerce.Domain.Service;

using Common.Domain;
using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.DataTransfer;

public interface IProductUpdaterService
{
    Task<Result<ResultUnit, ProblemDetailsException>> UpdateProduct(
        Guid id,
        UpdateProductCommand command,
        CancellationToken cancellationToken);
}