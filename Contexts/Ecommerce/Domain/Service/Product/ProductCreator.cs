namespace Ecommerce.Domain;

public interface IProductCreatorService
{
    public Task<OneOf<Guid, ProblemDetailsException>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
}