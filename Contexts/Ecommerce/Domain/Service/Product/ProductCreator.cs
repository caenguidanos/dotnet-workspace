namespace Ecommerce.Domain;

public interface IProductCreatorService
{
    public Task<OneOf<Guid, ProblemDetailsException>>
        AddNewProduct(Guid id, string title, string description, string status, int price, CancellationToken cancellationToken);
}