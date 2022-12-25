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

    public async Task<OneOf<Guid, ProblemDetailsException>> AddNewProduct(Guid id, string title, string description, int status, int price,
        CancellationToken cancellationToken)
    {
        Product newProduct;
        try
        {
            newProduct = new Product
            {
                Id = new ProductId(id),
                Title = new ProductTitle(title),
                Description = new ProductDescription(description),
                Status = new ProductStatus(status),
                Price = new ProductPrice(price)
            };
        }
        catch (ProblemDetailsException problemDetailsException)
        {
            return problemDetailsException;
        }

        var saveProductResult = await _productRepository.Save(newProduct, cancellationToken);

        return await saveProductResult.Match<Task<OneOf<Guid, ProblemDetailsException>>>(
            async _ =>
            {
                var productPrimitives = newProduct.ToPrimitives();

                var productCreatedEvent = new ProductCreatedEvent { Product = productPrimitives.Id };
                await _publisher.Publish(productCreatedEvent, cancellationToken);

                return productPrimitives.Id;
            },
            async problemDetailsException => await Task.FromResult(problemDetailsException)
        );
    }
}