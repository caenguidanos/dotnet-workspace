namespace Ecommerce.Store.Application.Service;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Notification;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Domain.ValueObject;
using Ecommerce.Store.Infrastructure.DataTransfer;

public class ProductService : IProductService
{
    private readonly IPublisher _publisher;
    private readonly IProductRepository _productRepository;

    public ProductService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        var newId = Product.NewID();

        var newProduct = new Product(
            new ProductId(newId),
            new ProductTitle(title),
            new ProductDescription(description),
            new ProductStatus((ProductStatusValue)status),
            new ProductPrice(price));

        await _productRepository.Save(newProduct, cancellationToken);

        await _publisher.Publish(new ProductCreatedNotification { Id = newId }, cancellationToken);

        return newProduct.Id;
    }

    public async Task DeleteProductById(Guid id, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteById(id, cancellationToken);

        await _publisher.Publish(new ProductRemovedNotification { Id = id }, cancellationToken);
    }

    public async Task UpdateProductById(Guid id, PartialProduct partialProduct, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetById(id, cancellationToken);

        string newProductTitle = partialProduct.Title ?? existingProduct.Title;
        string newProductDescription = partialProduct.Description ?? existingProduct.Description;
        int newProductStatus = partialProduct.Status ?? (int)existingProduct.Status;
        int newProductPrice = partialProduct.Price ?? existingProduct.Price;

        var existingProductWithUpdates = new Product(
            new ProductId(id),
            new ProductTitle(newProductTitle),
            new ProductDescription(newProductDescription),
            new ProductStatus((ProductStatusValue)newProductStatus),
            new ProductPrice(newProductPrice));

        await _productRepository.Update(existingProductWithUpdates, cancellationToken);

        await _publisher.Publish(new ProductUpdatedNotification { Id = id }, cancellationToken);
    }
}
