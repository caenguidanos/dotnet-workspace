namespace Ecommerce.Store.Infrastructure.Controller;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Application.Query;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Domain.LogEvent;
using Ecommerce.Store.Infrastructure.DTO;
using Common.Domain.Service;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILoggerService _logger;
    private readonly IMediator _mediator;

    public ProductController(ILoggerService logger, IMediator mediator)
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
            var query = new GetProductsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            _logger.Print(ProductLogEvent.GetAllNotImplemented, exception.Message);

            return StatusCode(StatusCodes.Status501NotImplemented);
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

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                _logger.Print(ProductLogEvent.GetByIdNotFound, exception.Message);

                return NotFound();
            }

            _logger.Print(ProductLogEvent.GetByIdNotImplemented, exception.Message);

            return StatusCode(StatusCodes.Status501NotImplemented);
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

            var result = await _mediator.Send(command, cancellationToken);

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
                _logger.Print(ProductLogEvent.CreateBadRequest, exception.Message);

                return BadRequest();
            }

            _logger.Print(ProductLogEvent.CreateNotImplemented, exception.Message);

            return StatusCode(StatusCodes.Status501NotImplemented);
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

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                _logger.Print(ProductLogEvent.DeleteByIdNotFound, exception.Message);

                return NotFound();
            }

            _logger.Print(ProductLogEvent.DeleteByIdNotImplemented, exception.Message);

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
