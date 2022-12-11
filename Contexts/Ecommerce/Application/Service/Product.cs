namespace Ecommerce.Application.Service;

using MediatR;

using Ecommerce.Application.Command;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Domain.ValueObject;

public sealed class ProductService : IProductService
{
    private IPublisher _publisher { get; init; }
    private IProductRepository _productRepository { get; init; }

    public ProductService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        var newId = Common.Domain.Schema.NewID();

        var newProduct = new Product
        {
            Id = new ProductId(newId),
            Title = new ProductTitle(title),
            Description = new ProductDescription(description),
            Status = new ProductStatus((ProductStatusValue)status),
            Price = new ProductPrice(price)
        };

        await _productRepository.Save(newProduct, cancellationToken);

        await _publisher.Publish(new ProductCreatedEvent { Product = newId }, cancellationToken);

        return newProduct.Id.GetValue();
    }

    public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        await _productRepository.Delete(id, cancellationToken);

        await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);
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
