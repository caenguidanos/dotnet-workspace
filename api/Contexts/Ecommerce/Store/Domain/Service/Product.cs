namespace api.Contexts.Ecommerce.Store.Domain.Service
{
    public interface IProductService
    {
        Task<string> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);

        Task DeleteProductById(string id, CancellationToken cancellationToken);
    }
}