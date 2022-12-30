namespace Ecommerce.Infrastructure;

public sealed class GetProductByIdHttpHandler
{
    private ISender _sender { get; }

    public GetProductByIdHttpHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var instance = context.Request.Path;

        var query = new GetProductQuery { Id = id };

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            data => Results.Json(data, options: Json.HttpSerializerOptions),
            error => Results.Problem(error.ToProblemDetails(instance))
        );
    }
}