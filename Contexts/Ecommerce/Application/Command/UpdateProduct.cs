namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Service;

public readonly struct UpdateProductCommand : IRequest<HttpResultResponse>
{
    public Guid Id { get; init; }
    public int? Price { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Status { get; init; }
}

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, HttpResultResponse>
{
    private readonly IProductUpdaterService _productUpdaterService;

    public UpdateProductHandler(IProductUpdaterService productUpdaterService)
    {
        _productUpdaterService = productUpdaterService;
    }

    public async Task<HttpResultResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await _productUpdaterService.UpdateProduct(request.Id, request, cancellationToken);

        return new HttpResultResponse()
        {
            StatusCode = HttpStatusCode.Accepted,
        };
    }
}
