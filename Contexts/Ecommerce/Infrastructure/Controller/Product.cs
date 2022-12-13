namespace Ecommerce.Infrastructure.Controller;

using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Common.Application;

using Ecommerce.Application.Command;
using Ecommerce.Application.Query;
using Ecommerce.Infrastructure.DataTransfer;

[ApiController]
[Route("[controller]")]
public sealed class ProductController : ControllerBase
{
    private ISender _sender { get; init; }
    private IExceptionManager _exceptionManager { get; init; }

    public ProductController(ISender sender, IExceptionManager exceptionManager)
    {
        _sender = sender;
        _exceptionManager = exceptionManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async ValueTask<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductsQuery();

            var payload = await _sender.Send(query, cancellationToken);

            return new HttpResultResponse
            {
                Body = payload
            };
        }
        catch (Exception ex)
        {
            return _exceptionManager.HandleHttp(ex);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async ValueTask<IActionResult> GetProductById([FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductQuery { Id = id };

            var payload = await _sender.Send(query, cancellationToken);

            return new HttpResultResponse
            {
                Body = payload
            };
        }
        catch (Exception ex)
        {
            return _exceptionManager.HandleHttp(ex);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async ValueTask<IActionResult> CreateProduct([FromBody] CreateProductHttpRequestBody body, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateProductCommand
            {
                Title = body.Title,
                Description = body.Description,
                Price = body.Price,
                Status = body.Status,
            };

            var payload = await _sender.Send(command, cancellationToken);

            return new HttpResultResponse
            {
                Body = payload
            };
        }
        catch (Exception ex)
        {
            return _exceptionManager.HandleHttp(ex);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async ValueTask<IActionResult> RemoveProduct([FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new RemoveProductCommand { Id = id };

            await _sender.Send(command, cancellationToken);

            return new HttpResultResponse
            {
                StatusCode = HttpStatusCode.Accepted
            };
        }
        catch (Exception ex)
        {
            return _exceptionManager.HandleHttp(ex);
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async ValueTask<IActionResult> UpdateProduct([FromRoute(Name = "id")] Guid id, [FromBody] UpdateProductHttpRequestBody body, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Title = body.Title,
                Description = body.Description,
                Price = body.Price,
                Status = body.Status
            };

            await _sender.Send(command, cancellationToken);

            return new HttpResultResponse
            {
                StatusCode = HttpStatusCode.Accepted
            };
        }
        catch (Exception ex)
        {
            return _exceptionManager.HandleHttp(ex);
        }
    }
}
