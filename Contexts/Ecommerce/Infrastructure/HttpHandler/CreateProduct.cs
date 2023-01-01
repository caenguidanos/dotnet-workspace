namespace Ecommerce.Infrastructure.HttpHandler;

using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class CreateProductHttpHandler
{
    private ISender _sender { get; }

    public CreateProductHttpHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IResult> HandleAsync(HttpContext context, [FromBody] CreateProductHttpRequestBody body, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        {
            Id = body.Id,
            Title = body.Title,
            Description = body.Description,
            Price = body.Price,
            Status = body.Status
        };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            _ => Results.Accepted(),
            error => Results.Problem(error.ToProblemDetails(context.Request.Path))
        );
    }
}