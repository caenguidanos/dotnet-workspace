namespace Ecommerce.Infrastructure;

public sealed class RemoveProductByIdEndpoint : IRemoveProductByIdEndpoint
{
    private ISender _sender { get; }

    public RemoveProductByIdEndpoint(ISender sender)
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
                exception.AsProblemDetails(out var problemDetails);

                return Results.Problem(problemDetails);
            }
        );
    }
}