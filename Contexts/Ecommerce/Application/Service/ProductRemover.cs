namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

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

    public async Task<Result<bool>> RemoveProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productRepository.Delete(id, cancellationToken);

        if (result.Err is not null)
        {
            return new Result<bool> { Err = result.Err };
        }

        await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);

        return new Result<bool> { };
    }
}
