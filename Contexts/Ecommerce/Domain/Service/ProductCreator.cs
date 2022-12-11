namespace Ecommerce.Domain.Service;

public interface IProductCreatorService
{
    Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
}
