using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Event;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Domain.Repository;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.ValueObject;
using MediatR;

namespace api.Contexts.Ecommerce.Store.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IPublisher _publisher;
        private readonly IProductRepository _productRepository;


        public ProductService(IPublisher publisher, IProductRepository productRepository)
        {
            _publisher = publisher;
            _productRepository = productRepository;
        }

        public string AddNewProduct(string id, string title, string description, int status, int price)
        {
            var newProduct = new Product(
                new ProductId(id),
                new ProductTitle(title),
                new ProductDescription(description),
                new ProductStatus((ProductStatusValue)status),
                new ProductPrice(price)
            );

            _productRepository.Add(newProduct);

            var productCreatedEvent = new ProductCreatedEvent { Id = id };
            _publisher.Publish<ProductCreatedEvent>(productCreatedEvent);

            return newProduct.Id;
        }

        public void DeleteProductById(string id)
        {
            _productRepository.Delete(id);

            var productRemovedEvent = new ProductRemovedEvent { Id = id };
            _publisher.Publish<ProductRemovedEvent>(productRemovedEvent);
        }
    }
}

