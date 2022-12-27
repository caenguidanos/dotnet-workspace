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

        return await getProductByIdResult.Match<ValueTask<OneOf<byte, ProblemDetailsException>>>(
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
                        Status = new ProductStatus(command.Status ?? currentProductPrimitives.Status),
                        Price = new ProductPrice(command.Price ?? currentProductPrimitives.Price)
                    };
                }
                catch (ProblemDetailsException p)
                {
                    return p;
                }

                var updateProductResult = await _productRepository.Update(updatedProduct, cancellationToken);

                return await updateProductResult.Match<ValueTask<OneOf<byte, ProblemDetailsException>>>(
                    async _ =>
                    {
                        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);

                        return default;
                    },
                    async p => p
                );
            },
            async p => p
        );
    }
}