namespace api.Contexts.Ecommerce.Store.Domain.Service
{
    public interface IProductService
    {
        string AddNewProduct(string id, string title, string description, int status, int price);
        void DeleteProductById(string id);
    }
}