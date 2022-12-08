namespace Ecommerce.Infrastructure.Controller;

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
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductsQuery();
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpGet("Event")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductsEventsQuery();
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetById([FromRoute(Name = "id")] Guid Id, CancellationToken cancellationToken)
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

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> Create([FromBody] ProductPrimitivesForCreateOperation request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateProductCommand
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Status = request.Status,
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
                return BadRequest();
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> DeleteById([FromRoute(Name = "id")] Guid Id, CancellationToken cancellationToken)
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

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> UpdateById([FromRoute(Name = "id")] Guid Id, [FromBody] ProductPrimitivesForUpdateOperation partialProduct, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateProductCommand
            {
                Id = Id,
                Title = partialProduct.Title,
                Description = partialProduct.Description,
                Price = partialProduct.Price,
                Status = partialProduct.Status
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

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
