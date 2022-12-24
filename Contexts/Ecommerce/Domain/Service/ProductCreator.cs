namespace Ecommerce.Domain;

public interface IProductCreatorService
{
    Task<OneOf<Guid, ProblemDetailsException>> AddNewProduct(string title, string description, int status, int price, CancellationToken cancellationToken);
}