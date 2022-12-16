namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

using Ecommerce.Application.Command;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Error;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Domain.ValueObject;

public sealed class ProductUpdaterService : IProductUpdaterService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductUpdaterService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Result<byte, ProductError>> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var getByIdResult = await _productRepository.GetById(id, cancellationToken);
        if (getByIdResult.IsFaulted)
        {
            return new Result<byte, ProductError>(getByIdResult.Err);
        }

        var previousProductState = getByIdResult.Ok.ToPrimitives();

        Product product;
        try
        {
            var nextProductId = new ProductId(id);
            var nextProductTitle = new ProductTitle(command.Title ?? previousProductState.Title);
            var nextProductDescription = new ProductDescription(command.Description ?? previousProductState.Description);
            var nextProductStatus = new ProductStatus((ProductStatusValue)(command.Status ?? (int)previousProductState.Status));
            var nextProductPrice = new ProductPrice(command.Price ?? previousProductState.Price);

            product = new Product
            {
                Id = nextProductId,
                Title = nextProductTitle,
                Description = nextProductDescription,
                Status = nextProductStatus,
                Price = nextProductPrice
            };
        }
        catch (ProductError ex)
        {
            return new Result<byte, ProductError>(ex);
        }

        var updateResult = await _productRepository.Update(product, cancellationToken);
        if (updateResult.IsFaulted)
        {
            return new Result<byte, ProductError>(updateResult.Err);
        }

        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);

        return new Result<byte, ProductError>();
    }
}
