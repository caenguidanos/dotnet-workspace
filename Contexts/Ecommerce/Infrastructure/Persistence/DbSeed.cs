namespace Ecommerce.Infrastructure.Persistence;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.ValueObject;

public class DbSeed
{
    private readonly DbContext _dbContext;

    public DbSeed(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Run()
    {
        using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());

        conn.Open();
        TruncateTable(conn, "product");
        TruncateTable(conn, "event");
        InsertProducts(conn);
        conn.Close();
    }

    private static void TruncateTable(NpgsqlConnection conn, string name)
    {
        string sql = $"TRUNCATE public.{name}";
        var command = new CommandDefinition(sql);
        conn.Execute(command);
    }

    private static void InsertProducts(NpgsqlConnection conn)
    {
        var products = new List<Product>
        {
            new Product(
                new ProductId(Common.Domain.Schema.NewID()),
                new ProductTitle("American Professional II Stratocaster"),
                new ProductDescription("Great guitar"),
                new ProductStatus(ProductStatusValue.Published),
                new ProductPrice(2_199 * 100)
            ),
            new Product(
                new ProductId(Common.Domain.Schema.NewID()),
                new ProductTitle("Mustang Shelby GT500"),
                new ProductDescription("Great car"),
                new ProductStatus(ProductStatusValue.Published),
                new ProductPrice(79_000 * 100)
            ),
            new Product(
                new ProductId(Common.Domain.Schema.NewID()),
                new ProductTitle("Antelope Orion +32"),
                new ProductDescription("Great audio interface"),
                new ProductStatus(ProductStatusValue.Published),
                new ProductPrice(3_000 * 100)
            ),
        };

        products.ForEach(product =>
        {
            {
                string sql = "INSERT INTO public.product (id, title, description, price, status) VALUES (@Id, @Title, @Description, @Price, @Status)";
                var parameters = new DynamicParameters();
                parameters.Add("Id", product.Id);
                parameters.Add("Title", product.Title);
                parameters.Add("Description", product.Description);
                parameters.Add("Price", product.Price);
                parameters.Add("Status", product.Status);
                var command = new CommandDefinition(sql, parameters);
                conn.Execute(command);
            }
        });
    }
}
