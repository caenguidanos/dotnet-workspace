using api.Contexts.Ecommerce.Store.Domain.Entity;

namespace api.Contexts.Ecommerce.Store.Domain.Service
{
    public interface IProductService
    {
        public abstract Product? Get(int id);

        public abstract List<Product> GetAll();

        public abstract void Add(Product product);

        public abstract void Delete(int id);
    }
}