namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.AspNetCore.Http;

using Common.Application.HttpUtil;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Service;

public class DeleteProductCommand : IRequest<HttpResultResponse>
{
    public required Guid Id { get; init; }
}

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, HttpResultResponse>
{
    private readonly IProductService productService;

    public DeleteProductHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<HttpResultResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await productService.DeleteProduct(request.Id, cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                StatusCode = StatusCodes.Status202Accepted,
            };
        }
        catch (Exception ex)
        {
            if (ex is ProductNotFoundException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            if (ex is ProductPersistenceException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable,
                };
            }

            return new HttpResultResponse(cancellationToken)
            {
                StatusCode = StatusCodes.Status501NotImplemented,
            };
        }
    }
}
