namespace Ecommerce.Infrastructure;

public sealed class UpdateProductHttpHandler
{
    private ISender _sender { get; }

    public UpdateProductHttpHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, [FromBody] UpdateProductHttpRequestBody body,
        CancellationToken cancellationToken)
    {
        var instance = context.Request.Path;

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
            error => Results.Problem(error.ToProblemDetails(instance))
        );
    }
}