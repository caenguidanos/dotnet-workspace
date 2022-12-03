using api.Contexts.Ecommerce.Store.Domain.Entity;

namespace api.Contexts.Ecommerce.Store.Domain.Repository
{
    public interface IProductRepository
    {
        public abstract Product Get(string id);

        public abstract IEnumerable<Product> GetAll();

        public abstract void Add(Product product);

        public abstract void Delete(string id);
    }
}