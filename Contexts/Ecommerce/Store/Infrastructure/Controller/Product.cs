namespace Ecommerce.Store.Infrastructure.Controller;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Application.Query;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Infrastructure.DTO;

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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductsQuery();
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status501NotImplemented, exception.ToString());
        }
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
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
                return NotFound(exception.ToString());
            }

            return StatusCode(StatusCodes.Status501NotImplemented, exception.ToString());
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> Create([FromBody] NewProduct request, CancellationToken cancellationToken)
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

            var result = await _sender.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception
                is ProductIdInvalidException
                or ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
            {
                return BadRequest(exception.ToString());
            }

            return StatusCode(StatusCodes.Status501NotImplemented, exception.ToString());
        }
    }

    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> DeleteById([FromRoute(Name = "id")] Guid Id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteProductCommand { Id = Id };

            var result = await _sender.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                return NotFound(exception.ToString());
            }

            return StatusCode(StatusCodes.Status501NotImplemented, exception.ToString());
        }
    }
}
