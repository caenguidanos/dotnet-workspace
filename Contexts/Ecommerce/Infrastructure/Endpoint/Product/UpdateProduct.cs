namespace Ecommerce.Infrastructure;

public sealed class UpdateProductEndpoint : IUpdateProductEndpoint
{
    private ISender _sender { get; }

    public UpdateProductEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, [FromBody] UpdateProductHttpRequestBody body,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand
        {
            Id = id,
            Title = body.Title,
            Description = body.Description,
            Price = body.Price,
            Status = body.Status
        };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            _ => Results.Accepted(),
            p =>
            {
                p.SetInstance(context.Request.Path);
                p.AsProblemDetails(out var problemDetails);
                return Results.Problem(problemDetails);
            }
        );
    }
}