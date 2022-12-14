namespace Ecommerce.Application.Service;

using Mediator;

using Common.Domain;

using Ecommerce.Application.Command;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;

public sealed class ProductUpdaterService : IProductUpdaterService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductUpdaterService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Result> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var getByIdResult = await _productRepository.GetById(id, cancellationToken);
        if (getByIdResult.Err is not null)
        {
            return new Result(getByIdResult.Err);
        }

        if (getByIdResult.Ok is null)
        {
            throw new ArgumentNullException();
        }

        var product = getByIdResult.Ok;

        var updatedProduct = new Product
        {
            Id = id,
            Title = command.Title ?? product.Title,
            Description = command.Description ?? product.Description,
            Status = (ProductStatusValue)(command.Status ?? (int)product.Status),
            Price = command.Price ?? product.Price
        };

        if (updatedProduct.HasError())
        {
            return new Result(updatedProduct.GetError());
        }

        var updateResult = await _productRepository.Update(updatedProduct, cancellationToken);
        if (updateResult.Err is not null)
        {
            return new Result(updateResult.Err);
        }

        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);

        return new Result();
    }
}
