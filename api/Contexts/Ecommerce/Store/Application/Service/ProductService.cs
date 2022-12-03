using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Domain.ValueObject;
using api.Contexts.Ecommerce.Store.Domain.Service;

namespace api.Contexts.Ecommerce.Store.Application.Service
{
    public class ProductService : IProductService
    {
        public List<Product> Products { get; }

        public ProductService()
        {
            Products = new List<Product>
            {
                new Product(
                    new ProductId(1),
                    new ProductTitle("Titulo 1"),
                    new ProductDescription("Description 1"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(100)
                ),
                new Product(
                    new ProductId(2),
                    new ProductTitle("Titulo 2"),
                    new ProductDescription("Description 2"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(200)
                ),
                new Product(
                    new ProductId(3),
                    new ProductTitle("Titulo 3"),
                    new ProductDescription("Description 3"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(300)
                ),
                new Product(
                    new ProductId(4),
                    new ProductTitle("Titulo 4"),
                    new ProductDescription("Description 4"),
                    new ProductStatus(ProductStatusValue.Published),
                    new ProductPrice(400)
                ),
            };
        }

        public Product Get(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }

        public List<Product> GetAll()
        {
            return Products;
        }

        public void Add(Product product)
        {
            Products.Add(product);
        }

        public void Delete(int id)
        {
            var selectedProduct = Get(id);

            if (selectedProduct is null) return;

            Products.Remove(selectedProduct);
        }
    }
}

