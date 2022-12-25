namespace Ecommerce.Domain;

public interface IDeleteProductEndpoint
{
    public Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken);
}