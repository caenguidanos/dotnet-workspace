namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;

public sealed class ProductCreatorService : IProductCreatorService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductCreatorService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Id = Schema.NewID(),
            Title = title,
            Description = description,
            Status = (ProductStatusValue)status,
            Price = price
        };

        if (newProduct.HasError())
        {
            return new Result<Guid> { Err = newProduct.GetError() };
        }

        var saveResult = await _productRepository.Save(newProduct, cancellationToken);

        if (saveResult.Err is not null)
        {
            return new Result<Guid> { Err = saveResult.Err };
        }

        await _publisher.Publish(new ProductCreatedEvent { Product = newProduct.Id }, cancellationToken);

        return new Result<Guid> { Ok = newProduct.Id };
    }
}
