namespace Ecommerce.Infrastructure;

public sealed class DeleteProductEndpoint : IDeleteProductEndpoint
{
    private ISender _sender { get; }

    public DeleteProductEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveProductCommand { Id = id };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            _ => Results.Accepted(),
            exception =>
            {
                exception.SetInstance(context.Request.Path);
                exception.TryProblemDetailsPayload(out var payload);
                return Results.Problem(payload);
            }
        );
    }
}