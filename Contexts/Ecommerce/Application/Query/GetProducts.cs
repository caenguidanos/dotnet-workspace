namespace Ecommerce.Application.Query;

using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

using Common.Application.HttpUtil;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Repository;

public class GetProductsQuery : IRequest<HttpResultResponse>
{
}

public class GetProductsHandler : IRequestHandler<GetProductsQuery, HttpResultResponse>
{
    private readonly IProductRepository productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productRepository.Get(cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                Body = products,
                StatusCode = StatusCodes.Status200OK,
                ContentType = MediaTypeNames.Application.Json,
            };
        }
        catch (Exception ex)
        {
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
