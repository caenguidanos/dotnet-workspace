namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

using Ecommerce.Application.Command;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class ProductUpdaterService : IProductUpdaterService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductUpdaterService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Result<ProductAck, ProblemDetailsException>> UpdateProduct(
        Guid id,
        UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var getProductByIdResult = await _productRepository.GetById(id, cancellationToken);
        if (getProductByIdResult.IsFaulted)
        {
            return new Result<ProductAck, ProblemDetailsException>(getProductByIdResult.Error);
        }

        var currentProduct = getProductByIdResult.Value.ToPrimitives();

        var nextProductId = new ProductId(id);
        var nextProductTitle = new ProductTitle(command.Title ?? currentProduct.Title);
        var nextProductDescription = new ProductDescription(command.Description ?? currentProduct.Description);
        var nextProductStatus = new ProductStatus((ProductStatusValue)(command.Status ?? (int)currentProduct.Status));
        var nextProductPrice = new ProductPrice(command.Price ?? currentProduct.Price);
        var nextProduct = new Product
        {
            Id = nextProductId,
            Title = nextProductTitle,
            Description = nextProductDescription,
            Status = nextProductStatus,
            Price = nextProductPrice
        };

        var productIntegrityResult = nextProduct.CheckIntegrity();
        if (productIntegrityResult.IsFaulted)
        {
            return new Result<ProductAck, ProblemDetailsException>(productIntegrityResult.Error);
        }

        var updateProductResult = await _productRepository.Update(nextProduct, cancellationToken);
        if (updateProductResult.IsFaulted)
        {
            return new Result<ProductAck, ProblemDetailsException>(updateProductResult.Error);
        }

        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);
        return new Result<ProductAck, ProblemDetailsException>(new ProductAck { Id = id });
    }
}
