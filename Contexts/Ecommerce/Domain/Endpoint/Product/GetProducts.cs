namespace Ecommerce.Domain;

public interface IGetProductsEndpoint
{
    public Task<IResult> HandleAsync(HttpContext context, CancellationToken cancellationToken);
}