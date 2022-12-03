using Microsoft.AspNetCore.Mvc;
using MediatR;
using api.Contexts.Ecommerce.Store.Application.Query;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Application.DTO;
using api.Contexts.Ecommerce.Store.Application.Command;

namespace api.Contexts.Ecommerce.Store.Infrastructure.Controller;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetProductsQuery();

            var result = await _mediator.Send(request, cancellationToken);

            return Ok(result);
        }
        catch (System.Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetProductQuery
            {
                Id = id
            };

            var result = await _mediator.Send(request, cancellationToken);

            return Ok(result);
        }
        catch (System.Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var request = new CreateProductCommand
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Status = dto.Status
            };

            var result = await _mediator.Send(request, cancellationToken);

            return Ok(result);
        }
        catch (System.Exception exception)
        {
            if (
                exception
                is ProductIdInvalidException
                or ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException
                )
            {
                return BadRequest();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteProductCommand
            {
                Id = id
            };

            var result = await _mediator.Send(request, cancellationToken);

            return Ok(result);
        }
        catch (System.Exception exception)
        {
            if (exception is ProductNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

