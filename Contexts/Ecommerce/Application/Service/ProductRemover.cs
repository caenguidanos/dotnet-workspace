namespace Ecommerce.Application.Service;

using Ecommerce.Application.Event;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;

public sealed class ProductRemoverService : IProductRemoverService
{
    private IPublisher _publisher { get; }
    private IProductRepository _productRepository { get; }

    public ProductRemoverService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<OneOf<byte, ProblemDetailsException>> RemoveProduct(Guid id, CancellationToken cancellationToken)
    {
        var deleteProductResult = await _productRepository.Delete(id, cancellationToken);

        return await deleteProductResult.Match<ValueTask<OneOf<byte, ProblemDetailsException>>>(
            async _ =>
            {
                await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);

                return default;
            },
            async error => await ValueTask.FromResult(error)
        );
    }
}