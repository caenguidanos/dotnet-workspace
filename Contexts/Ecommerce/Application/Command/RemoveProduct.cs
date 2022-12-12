namespace Ecommerce.Application.Command;

using MediatR;
using System.Net;

using Common.Application.HttpUtil;
using Ecommerce.Domain.Service;

public readonly struct RemoveProductCommand : IRequest<HttpResultResponse>
{
    public required Guid Id { get; init; }
}

public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, HttpResultResponse>
{
    private readonly IProductRemoverService _productRemoverService;

    public RemoveProductHandler(IProductRemoverService productRemoverService)
    {
        _productRemoverService = productRemoverService;
    }

    public async Task<HttpResultResponse> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        await _productRemoverService.RemoveProduct(request.Id, cancellationToken);

        return new HttpResultResponse()
        {
            StatusCode = HttpStatusCode.Accepted,
        };
    }
}
