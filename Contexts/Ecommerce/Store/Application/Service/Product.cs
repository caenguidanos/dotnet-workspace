namespace Ecommerce.Store.Application.Service;

using Ecommerce.Store.Application.Event;
using Ecommerce.Store.Domain.Entity;
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
        var newId = Common.Domain.Entity.Entity.NewID();

        var newProduct = new Product(
            new ProductId(newId),
            new ProductTitle(title),
            new ProductDescription(description),
            new ProductStatus((ProductStatusValue)status),
            new ProductPrice(price));

        await _productRepository.Save(newProduct, cancellationToken);

        await _publisher.Publish(new ProductCreatedEvent { Product = newId }, cancellationToken);

        return newProduct.Id;
    }

    public async Task DeleteProductById(Guid id, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteById(id, cancellationToken);

        await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);
    }

    public async Task UpdateProductById(Guid id, ProductPrimitivesForUpdateOperation product, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetById(id, cancellationToken);

        string newProductTitle = product.Title ?? existingProduct.Title;
        string newProductDescription = product.Description ?? existingProduct.Description;
        int newProductStatus = product.Status ?? (int)existingProduct.Status;
        int newProductPrice = product.Price ?? existingProduct.Price;

        var existingProductWithUpdates = new Product(
            new ProductId(id),
            new ProductTitle(newProductTitle),
            new ProductDescription(newProductDescription),
            new ProductStatus((ProductStatusValue)newProductStatus),
            new ProductPrice(newProductPrice));

        await _productRepository.Update(existingProductWithUpdates, cancellationToken);

        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);
    }
}
