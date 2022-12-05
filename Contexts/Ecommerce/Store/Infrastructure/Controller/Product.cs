namespace Ecommerce.Store.Infrastructure.Controller;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Application.Query;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Infrastructure.Model;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllQuery();

            var result = await mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetByIdQuery
            {
                Id = id,
            };

            var result = await mediator.Send(query, cancellationToken);

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequestBodyDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateProductCommand
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Status = dto.Status,
            };

            var result = await mediator.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (
                exception
                is ProductIdInvalidException
                or ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
            {
                return BadRequest();
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteProductCommand
            {
                Id = id,
            };

            var result = await mediator.Send(command, cancellationToken);

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
}
