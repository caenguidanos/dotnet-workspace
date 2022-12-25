namespace Ecommerce.Domain;

public interface IGetProductByIdEndpoint
{
    public Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken);
}