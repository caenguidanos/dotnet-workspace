namespace Ecommerce.Infrastructure;

public sealed class GetProductsEndpoint : IGetProductsEndpoint
{
    private ISender _sender { get; }

    public GetProductsEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var instance = context.Request.Path;

        var query = new GetProductsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            data => Results.Json(data, options: Json.HttpSerializerOptions),
            error => Results.Problem(error.ToProblemDetails(instance))
        );
    }
}