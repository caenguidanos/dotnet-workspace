using api.Contexts.Ecommerce.Store.Domain.Entity;

namespace api.Contexts.Ecommerce.Store.Domain.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();

        Task<Product> GetById(string id);

        Task Save(Product product);

        Task DeleteById(string id);

        Task<string> GenerateID();
    }
}