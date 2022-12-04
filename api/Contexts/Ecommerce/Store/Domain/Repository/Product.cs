using api.Contexts.Ecommerce.Store.Domain.Entity;

namespace api.Contexts.Ecommerce.Store.Domain.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken);

        Task<Product> GetById(string id, CancellationToken cancellationToken);

        Task Save(Product product, CancellationToken cancellationToken);

        Task DeleteById(string id, CancellationToken cancellationToken);

        Task<string> GenerateID(CancellationToken cancellationToken);
    }
}