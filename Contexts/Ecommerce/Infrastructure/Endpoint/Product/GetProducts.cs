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
            data => Results.Json(data, options: Json.HttpSerializerOptions),
            p =>
            {
                p.SetInstance(context.Request.Path);
                p.AsProblemDetails(out var problemDetails);
                return Results.Problem(problemDetails);
            }
        );
    }
}