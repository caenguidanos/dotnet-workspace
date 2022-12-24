namespace Ecommerce.Application.Service;

using Mediator;
using Common.Domain;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Domain.ValueObject;
using OneOf;

public sealed class ProductCreatorService : IProductCreatorService
{
    private IPublisher _publisher { get; }
    private IProductRepository _productRepository { get; }

    public ProductCreatorService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<OneOf<Guid, ProblemDetailsException>> AddNewProduct(
        string title,
        string description,
        int status,
        int price,
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = new ProductId(),
            Title = new ProductTitle(title),
            Description = new ProductDescription(description),
            Status = new ProductStatus((ProductStatusValue)status),
            Price = new ProductPrice(price)
        };

        try
        {
            product.CheckIntegrity();
        }
        catch (ProblemDetailsException ex)
        {
            return ex;
        }

        var saveProductResult = await _productRepository.Save(product, cancellationToken);

        return await saveProductResult.Match<Task<OneOf<Guid, ProblemDetailsException>>>(
            async _ =>
            {
                var productPrimitives = product.ToPrimitives();

                var productCreatedEvent = new ProductCreatedEvent { Product = productPrimitives.Id };
                await _publisher.Publish(productCreatedEvent, cancellationToken);

                return productPrimitives.Id;
            },
            async exception => await Task.FromResult(exception)
        );
    }
}