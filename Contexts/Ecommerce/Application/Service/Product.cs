namespace Contexts.Ecommerce.Application.Service;

using Contexts.Ecommerce.Application.Event;
using Contexts.Ecommerce.Domain.Entity;
using Contexts.Ecommerce.Domain.Model;
using Contexts.Ecommerce.Domain.Repository;
using Contexts.Ecommerce.Domain.Service;
using Contexts.Ecommerce.Domain.ValueObject;
using Contexts.Ecommerce.Infrastructure.DataTransfer;

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

        await _publisher.Publish(new ProductCreatedEvent { Product = newId }, cancellationToken);

        return newProduct.Id;
    }

    public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        await _productRepository.Delete(id, cancellationToken);

        await _publisher.Publish(new ProductRemovedEvent { Product = id }, cancellationToken);
    }

    public async Task UpdateProduct(Guid id, ProductPrimitivesForUpdateOperation product, CancellationToken cancellationToken)
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
