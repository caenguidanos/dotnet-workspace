namespace Ecommerce.Domain.Service;

using Common.Domain;

public interface IProductCreatorService
{
    Task<Result<Guid>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
}
