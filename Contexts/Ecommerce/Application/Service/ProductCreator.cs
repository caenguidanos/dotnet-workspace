namespace Ecommerce.Application.Service;

using Mediator;

using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Domain.ValueObject;

public sealed class ProductCreatorService : IProductCreatorService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductCreatorService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        var newId = Common.Domain.Schema.NewID();

        var newProduct = new Product
        {
            Id = new ProductId(newId),
            Title = new ProductTitle(title),
            Description = new ProductDescription(description),
            Status = new ProductStatus((ProductStatusValue)status),
            Price = new ProductPrice(price)
        };

        await _productRepository.Save(newProduct, cancellationToken);

        await _publisher.Publish(new ProductCreatedEvent { Product = newId }, cancellationToken);

        return newProduct.Id.GetValue();
    }
}
