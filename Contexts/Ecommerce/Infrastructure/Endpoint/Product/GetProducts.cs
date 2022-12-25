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
        var query = new GetProductsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            products => Results.Json(products, options: Json.HttpSerializerOptions),
            exception =>
            {
                exception.SetInstance(context.Request.Path);
                exception.TryProblemDetails(out var payload);
                return Results.Problem(payload);
            }
        );
    }
}