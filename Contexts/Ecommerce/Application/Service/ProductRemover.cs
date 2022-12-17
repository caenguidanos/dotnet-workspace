namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

using Ecommerce.Application.Event;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class ProductRemoverService : IProductRemoverService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductRemoverService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Result<ProductAck, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken)
    {
        var deleteProductResult = await _productRepository.Delete(id, cancellationToken);
        if (deleteProductResult.IsFaulted)
        {
            return new Result<ProductAck, ProblemDetailsException>(deleteProductResult.Error);
        }

        await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);
        return new Result<ProductAck, ProblemDetailsException>(new ProductAck { Id = id });
    }
}
