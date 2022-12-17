namespace Ecommerce.Application.Service;

using Mediator;
using Common.Domain;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exception;
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

    public async Task<Result<Guid, ProblemDetailsException>> AddNewProduct(
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

        var productIntegrityResult = product.CheckIntegrity();
        if (productIntegrityResult.IsFaulted)
        {
            return new Result<Guid, ProblemDetailsException>(productIntegrityResult.Error);
        }

        var saveProductResult = await _productRepository.Save(product, cancellationToken);
        if (saveProductResult.IsFaulted)
        {
            return new Result<Guid, ProblemDetailsException>(saveProductResult.Error);
        }

        var productPrimitives = product.ToPrimitives();
        await _publisher.Publish(new ProductCreatedEvent { Product = productPrimitives.Id }, cancellationToken);
        return new Result<Guid, ProblemDetailsException>(productPrimitives.Id);
    }
}
