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

        public async Task<string> AddNewProduct(
            string title,
            string description,
            int status,
            int price,
            CancellationToken cancellationToken
            )
        {
            var newId = await _productRepository.GenerateID(cancellationToken);

            var newProduct = new Product(
                new ProductId(newId),
                new ProductTitle(title),
                new ProductDescription(description),
                new ProductStatus((ProductStatusValue)status),
                new ProductPrice(price)
            );

            await _productRepository.Save(newProduct, cancellationToken);

            var productCreatedEvent = new ProductCreatedEvent { Id = newId };
            await _publisher.Publish<ProductCreatedEvent>(productCreatedEvent);

            return newProduct.Id;
        }

        public async Task DeleteProductById(string id, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteById(id, cancellationToken);

            var productRemovedEvent = new ProductRemovedEvent { Id = id };
            await _publisher.Publish<ProductRemovedEvent>(productRemovedEvent);
        }
    }
}

