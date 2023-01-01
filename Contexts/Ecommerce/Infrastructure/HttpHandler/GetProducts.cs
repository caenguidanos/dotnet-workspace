namespace Ecommerce.Infrastructure.HttpHandler;

using Ecommerce.Application.Query;

public sealed class GetProductsHttpHandler
{
    private ISender _sender { get; }

    public GetProductsHttpHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            data => Results.Json(data, options: Json.OutHttpJsonSerializerOptions),
            error => Results.Problem(error.ToProblemDetails(context.Request.Path))
        );
    }
}