namespace Ecommerce.Application.Service;

using MediatR;

using Ecommerce.Application.Command;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
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

    public async Task UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetById(id, cancellationToken);

        string productTitle = command.Title ?? existingProduct.Title.GetValue();
        string productDescription = command.Description ?? existingProduct.Description.GetValue();
        int productStatus = command.Status ?? (int)existingProduct.Status.GetValue();
        int productPrice = command.Price ?? existingProduct.Price.GetValue();

        var existingProductWithUpdates = new Product
        {
            Id = new ProductId(id),
            Title = new ProductTitle(productTitle),
            Description = new ProductDescription(productDescription),
            Status = new ProductStatus((ProductStatusValue)productStatus),
            Price = new ProductPrice(productPrice)
        };

        await _productRepository.Update(existingProductWithUpdates, cancellationToken);

        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);
    }
}
