namespace Ecommerce.Application.Service;

using Mediator;
using Common.Domain;
using Ecommerce.Application.Command;
using Ecommerce.Application.Event;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Domain.ValueObject;
using OneOf;

public sealed class ProductUpdaterService : IProductUpdaterService
{
    private IPublisher _publisher { get; }
    private IProductRepository _productRepository { get; }

    public ProductUpdaterService(IPublisher publisher, IProductRepository productRepository)
    {
        _publisher = publisher;
        _productRepository = productRepository;
    }

    public async Task<OneOf<byte, ProblemDetailsException>> UpdateProduct(
        Guid id,
        UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var getProductByIdResult = await _productRepository.GetById(id, cancellationToken);

        return await getProductByIdResult.Match<Task<OneOf<byte, ProblemDetailsException>>>(
            async product =>
            {
                var currentProduct = product.ToPrimitives();

                var nextProduct = new Product
                {
                    Id = new ProductId(id),
                    Title = new ProductTitle(command.Title ?? currentProduct.Title),
                    Description = new ProductDescription(command.Description ?? currentProduct.Description),
                    Status = new ProductStatus((ProductStatusValue)(command.Status ?? (int)currentProduct.Status)),
                    Price = new ProductPrice(command.Price ?? currentProduct.Price)
                };

                try
                {
                    product.CheckIntegrity();
                }
                catch (ProblemDetailsException ex)
                {
                    return ex;
                }

                var updateProductResult = await _productRepository.Update(nextProduct, cancellationToken);

                return await updateProductResult.Match<Task<OneOf<byte, ProblemDetailsException>>>(
                    async _ =>
                    {
                        await _publisher.Publish(new ProductUpdatedEvent { Product = id }, cancellationToken);
                        return default;
                    },
                    async exception => await Task.FromResult(exception)
                );
            },
            async exception => await Task.FromResult(exception)
        );
    }
}