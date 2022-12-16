namespace Ecommerce.Domain.Service;

using Common.Domain;

using Ecommerce.Domain.Error;

public interface IProductCreatorService
{
    Task<Result<Guid, ProductError>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
}
