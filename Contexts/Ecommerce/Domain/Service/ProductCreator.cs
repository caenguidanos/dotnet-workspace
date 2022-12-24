namespace Ecommerce.Domain.Service;

using Common.Domain;
using OneOf;

public interface IProductCreatorService
{
    Task<OneOf<Guid, ProblemDetailsException>> AddNewProduct(
        string title,
        string description,
        int status,
        int price,
        CancellationToken cancellationToken);
}