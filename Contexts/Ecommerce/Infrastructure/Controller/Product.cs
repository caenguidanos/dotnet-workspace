namespace Ecommerce.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Ecommerce.Application.Command;
using Ecommerce.Application.Query;
using Ecommerce.Infrastructure.DataTransfer;

[ApiController]
[Route("[controller]")]
public sealed class ProductController : ControllerBase
{
    private ISender _sender { get; init; }

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery();

        var actionResult = await _sender.Send(query, cancellationToken);
        return actionResult;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetProductById([FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductQuery { Id = id };

        var actionResult = await _sender.Send(query, cancellationToken);
        return actionResult;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductHttpRequestBody body, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        {
            Title = body.Title,
            Description = body.Description,
            Price = body.Price,
            Status = body.Status,
        };

        var actionResult = await _sender.Send(command, cancellationToken);
        return actionResult;
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> RemoveProduct([FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveProductCommand { Id = id };

        var actionResult = await _sender.Send(command, cancellationToken);
        return actionResult;
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateProduct([FromRoute(Name = "id")] Guid id, [FromBody] UpdateProductHttpRequestBody body, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand
        {
            Id = id,
            Title = body.Title,
            Description = body.Description,
            Price = body.Price,
            Status = body.Status
        };

        var actionResult = await _sender.Send(command, cancellationToken);
        return actionResult;
    }
}
