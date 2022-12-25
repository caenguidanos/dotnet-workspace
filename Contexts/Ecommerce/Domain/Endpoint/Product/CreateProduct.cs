namespace Ecommerce.Domain;

public interface ICreateProductEndpoint
{
    public Task<IResult> HandleAsync(HttpContext context, [FromBody] CreateProductHttpRequestBody body, CancellationToken cancellationToken);
}