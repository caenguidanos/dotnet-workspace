namespace Ecommerce.Application;

using Mediator;

using Common.Domain;

using Domain;

using OneOf;

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

        return await deleteProductResult.Match<Task<OneOf<byte, ProblemDetailsException>>>(
            async _ =>
            {
                await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);
                return default;
            },
            async exception => await Task.FromResult(exception)
        );
    }
}