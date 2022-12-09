namespace Ecommerce.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Ecommerce.Application.Command;
using Ecommerce.Application.Query;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.DataTransfer;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

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
        try
        {
            var query = new GetProductsQuery();
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductRepositoryPersistenceException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetProductById([FromRoute(Name = "id")] Guid Id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductQuery { Id = Id };
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                return NotFound();
            }

            if (exception is ProductRepositoryPersistenceException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductHttpRequestBody body, CancellationToken cancellationToken)
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

            var ack = await _sender.Send(command, cancellationToken);

            return Accepted(ack);
        }
        catch (Exception exception)
        {
            if (exception
                is ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
            {
                return BadRequest(exception.ToString());
            }

            if (exception is ProductRepositoryPersistenceException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteProduct([FromRoute(Name = "id")] Guid Id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteProductCommand { Id = Id };
            await _sender.Send(command, cancellationToken);

            return Accepted();
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                return NotFound();
            }

            if (exception is ProductRepositoryPersistenceException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateProduct([FromRoute(Name = "id")] Guid Id, [FromBody] UpdateProductHttpRequestBody body, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateProductCommand
            {
                Id = Id,
                Title = body.Title,
                Description = body.Description,
                Price = body.Price,
                Status = body.Status
            };

            await _sender.Send(command, cancellationToken);

            return Accepted();
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                return NotFound();
            }

            if (exception
                is ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
            {
                return BadRequest();
            }

            if (exception is ProductRepositoryPersistenceException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
