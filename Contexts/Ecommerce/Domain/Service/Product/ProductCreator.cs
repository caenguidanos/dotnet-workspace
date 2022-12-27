namespace Ecommerce.Domain;

public interface IProductCreatorService
{
    public ValueTask<OneOf<Guid, ProblemDetailsException>>
        AddNewProduct(Guid id, string title, string description, string status, int price, CancellationToken cancellationToken);
}