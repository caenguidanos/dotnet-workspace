namespace api.Contexts.Ecommerce.Store.Domain.Service
{
    public interface IProductService
    {
        Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);

        Task DeleteProductById(Guid id, CancellationToken cancellationToken);
    }
}