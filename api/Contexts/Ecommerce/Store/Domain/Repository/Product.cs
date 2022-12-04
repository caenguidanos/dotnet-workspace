using api.Contexts.Ecommerce.Store.Domain.Entity;

namespace api.Contexts.Ecommerce.Store.Domain.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken);

        Task<Product> GetById(Guid id, CancellationToken cancellationToken);

        Task Save(Product product, CancellationToken cancellationToken);

        Task DeleteById(Guid id, CancellationToken cancellationToken);
    }
}