namespace Ecommerce.Infrastructure;

public sealed class GetProductByIdEndpoint : IGetProductByIdEndpoint
{
    private ISender _sender { get; }

    public GetProductByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductQuery { Id = id };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            product => Results.Json(product, options: Json.HttpSerializerOptions),
            exception =>
            {
                exception.SetInstance(context.Request.Path);
                exception.AsProblemDetails(out var problemDetails);

                return Results.Problem(problemDetails);
            }
        );
    }
}