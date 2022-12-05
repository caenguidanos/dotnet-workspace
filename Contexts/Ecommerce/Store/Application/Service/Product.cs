namespace Ecommerce.Store.Application.Service;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Notification;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Domain.ValueObject;

public class ProductService : IProductService
{
    private readonly IPublisher publisher;
    private readonly IProductRepository productRepository;

    public ProductService(IPublisher publisher, IProductRepository productRepository)
    {
        this.publisher = publisher;
        this.productRepository = productRepository;
    }

    public async Task<Guid> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        var id = Product.NewID();

        var newProduct = new Product(
            new ProductId(id),
            new ProductTitle(title),
            new ProductDescription(description),
            new ProductStatus((ProductStatusValue)status),
            new ProductPrice(price));

        await productRepository.Save(newProduct, cancellationToken);

        await publisher.Publish(new ProductCreatedNotification { Id = id }, cancellationToken);

        return newProduct.Id;
    }

    public async Task DeleteProductById(Guid id, CancellationToken cancellationToken)
    {
        await productRepository.DeleteById(id, cancellationToken);

        await publisher.Publish(new ProductRemovedNotification { Id = id }, cancellationToken);
    }
}
