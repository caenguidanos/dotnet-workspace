namespace Ecommerce.Domain;

public interface IRemoveProductByIdEndpoint
{
    public Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, CancellationToken cancellationToken);
}