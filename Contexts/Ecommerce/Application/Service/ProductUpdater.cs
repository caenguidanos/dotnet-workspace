namespace Ecommerce.Application;

public sealed class ProductUpdaterService : IProductUpdaterService
{
    private IPublisher _publisher { get; }
    private IProductRepository _productRepository { get; }

    public ProductUpdaterService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var getProductByIdResult = await _productRepository.GetById(id, cancellationToken);

        return await getProductByIdResult.Match<Task<OneOf<byte, ProblemDetailsException>>>(
            async currentProductPrimitives =>
            {
                Product updatedProduct;
                try
                {
                    updatedProduct = new Product
                    {
                        Id = new ProductId(id),
                        Title = new ProductTitle(command.Title ?? currentProductPrimitives.Title),
                        Description = new ProductDescription(command.Description ?? currentProductPrimitives.Description),
                        Status = new ProductStatus((ProductStatusValue)(command.Status ?? (int)currentProductPrimitives.Status)),
                        Price = new ProductPrice(command.Price ?? currentProductPrimitives.Price)
                    };
                }
                catch (ProblemDetailsException problemDetailsException)
                {
                    return problemDetailsException;
                }

                var updateProductResult = await _productRepository.Update(updatedProduct, cancellationToken);

                return await updateProductResult.Match<Task<OneOf<byte, ProblemDetailsException>>>(
                    async _ =>
                    {
                        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);

                        return default;
                    },
                    async problemDetailsException => await Task.FromResult(problemDetailsException)
                );
            },
            async problemDetailsException => await Task.FromResult(problemDetailsException)
        );
    }
}