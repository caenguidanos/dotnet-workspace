namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.AspNetCore.Http;

using Common.Application.HttpUtil;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Service;

public class UpdateProductCommand : IRequest<HttpResultResponse>
{
    public Guid Id { get; init; }
    public int? Price { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Status { get; init; }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, HttpResultResponse>
{
    private readonly IProductService productService;

    public UpdateProductHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<HttpResultResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await productService.UpdateProduct(request.Id, request, cancellationToken);

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

            if (ex
                is ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = StatusCodes.Status400BadRequest,
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
