using Microsoft.AspNetCore.Mvc;
using MediatR;
using api.Contexts.Ecommerce.Store.Application.Query;
using api.Contexts.Ecommerce.Store.Domain.Model;

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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var request = new GetProductQuery
            {
                Id = id
            };

            var result = await _mediator.Send(request);

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

