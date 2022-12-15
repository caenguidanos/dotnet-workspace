namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Error;
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

    public async Task<Result<Guid>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        Product product;
        try
        {
            product = new Product
            {
                Id = new ProductId(),
                Title = new ProductTitle(title),
                Description = new ProductDescription(description),
                Status = new ProductStatus((ProductStatusValue)status),
                Price = new ProductPrice(price)
            };
        }
        catch (ProductError ex)
        {
            return new Result<Guid> { Err = (IError)ex };
        }

        var saveResult = await _productRepository.Save(product, cancellationToken);
        if (saveResult.Err is not null)
        {
            return new Result<Guid> { Err = saveResult.Err };
        }

        await _publisher.Publish(
            new ProductCreatedEvent { Product = product.Id.GetValue() }, cancellationToken);

        return new Result<Guid> { Ok = product.Id.GetValue() };
    }
}
