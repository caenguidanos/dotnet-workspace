namespace Ecommerce.Infrastructure;

public sealed class RemoveProductByIdHttpHandler
{
    private ISender _sender { get; }

    public RemoveProductByIdHttpHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var instance = context.Request.Path;

        var command = new RemoveProductCommand { Id = id };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            _ => Results.Accepted(),
            error => Results.Problem(error.ToProblemDetails(instance))
        );
    }
}