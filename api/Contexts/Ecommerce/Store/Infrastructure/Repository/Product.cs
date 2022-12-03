using api.Contexts.Ecommerce.Store.Domain.Repository;
using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Domain.ValueObject;

namespace api.Contexts.Ecommerce.Store.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> Products { get; }

        public ProductRepository()
        {
            Products = new List<Product>
            {
                new Product(
                    new ProductId(Guid.NewGuid().ToString()),
                    new ProductTitle("Titulo 1"),
                    new ProductDescription("Description 1"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(100)
                ),
                new Product(
                    new ProductId(Guid.NewGuid().ToString()),
                    new ProductTitle("Titulo 2"),
                    new ProductDescription("Description 2"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(200)
                ),
                new Product(
                    new ProductId(Guid.NewGuid().ToString()),
                    new ProductTitle("Titulo 3"),
                    new ProductDescription("Description 3"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(300)
                ),
                new Product(
                    new ProductId(Guid.NewGuid().ToString()),
                    new ProductTitle("Titulo 4"),
                    new ProductDescription("Description 4"),
                    new ProductStatus(ProductStatusValue.Published),
                    new ProductPrice(400)
                ),
            };
        }

        public Product Get(string id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return Products;
        }

        public void Add(Product product)
        {
            Products.Add(product);
        }

        public void Delete(string id)
        {
            Products.Remove(Get(id));
        }
    }
}