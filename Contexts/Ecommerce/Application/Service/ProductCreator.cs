namespace Ecommerce.Application;

public sealed class ProductCreatorService : IProductCreatorService
{
    private IPublisher _publisher { get; }
    private IProductRepository _productRepository { get; }

    public ProductCreatorService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<OneOf<Guid, ProblemDetailsException>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken)
    {
        Product product;
        try
        {
            product = new Product
            {
                Id = new ProductId(),
                Title = new ProductTitle(title),
                Description = new ProductDescription(description),
                Status = new ProductStatus((ProductStatusValue)status),
                Price = new ProductPrice(price)
            };
        }
        catch (ProblemDetailsException ex)
        {
            return ex;
        }

        var saveProductResult = await _productRepository.Save(product, cancellationToken);

        return await saveProductResult.Match<Task<OneOf<Guid, ProblemDetailsException>>>(
            async _ =>
            {
                var productPrimitives = product.ToPrimitives();

                var productCreatedEvent = new ProductCreatedEvent { Product = productPrimitives.Id };
                await _publisher.Publish(productCreatedEvent, cancellationToken);

                return productPrimitives.Id;
            },
            async exception => await Task.FromResult(exception)
        );
    }
}