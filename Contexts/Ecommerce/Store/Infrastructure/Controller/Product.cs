namespace Ecommerce.Store.Infrastructure.Controller;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Application.Query;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Infrastructure.Model;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllProductsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            _logger.LogError(ProductLogEvent.GetAllNotImplemented, exception.Message);
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductByIdQuery
            {
                Id = id,
            };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                _logger.LogError(ProductLogEvent.GetByIdNotFound, exception.Message);
                return NotFound();
            }

            _logger.LogError(ProductLogEvent.GetByIdNotImplemented, exception.Message);
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpPost]
    [Produces("application/json")]
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

            var result = await _mediator.Send(command, cancellationToken);

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
                _logger.LogError(ProductLogEvent.CreateBadRequest, exception.Message);
                return BadRequest();
            }

            _logger.LogError(ProductLogEvent.CreateNotImplemented, exception.Message);
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteProductCommand
            {
                Id = id,
            };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                _logger.LogError(ProductLogEvent.DeleteByIdNotFound, exception.Message);
                return NotFound();
            }

            _logger.LogError(ProductLogEvent.DeleteByIdNotImplemented, exception.Message);
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
