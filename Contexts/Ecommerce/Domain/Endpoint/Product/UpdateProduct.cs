namespace Ecommerce.Domain;

public interface IUpdateProductEndpoint
{
    public Task<IResult> HandleAsync(HttpContext context, [FromRoute(Name = "id")] Guid id, [FromBody] UpdateProductHttpRequestBody body,
        CancellationToken cancellationToken);
}