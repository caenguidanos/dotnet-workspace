namespace Ecommerce.Application.Service;

using MediatR;

using Ecommerce.Application.Event;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;

public sealed class ProductRemoverService : IProductRemoverService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductRemoverService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task RemoveProduct(Guid id, CancellationToken cancellationToken)
    {
        await _productRepository.Delete(id, cancellationToken);

        await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);
    }
}
