namespace Ecommerce.Application.Query;

using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

using Common.Application.HttpUtil;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Repository;

public class GetProductQuery : IRequest<HttpResultResponse>
{
    public required Guid Id { get; init; }
}

public class GetProductHandler : IRequestHandler<GetProductQuery, HttpResultResponse>
{
    private readonly IProductRepository productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetById(request.Id, cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                Body = product,
                StatusCode = StatusCodes.Status200OK,
                ContentType = MediaTypeNames.Application.Json,
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
