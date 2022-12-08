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
        using var conn = _dbContext.CreateConnection();

        conn.Open();

        TruncateTable(conn, "product");
        TruncateTable(conn, "event");

        InsertProducts(conn);

        conn.Close();
    }

    private static void TruncateTable(NpgsqlConnection conn, string name)
    {
        string sql = $"TRUNCATE ecommerce.{name}";
        var command = new CommandDefinition(sql);
        conn.Execute(command);
    }

    private static void InsertProducts(NpgsqlConnection conn)
    {
        var products = new List<Product>
        {
            new Product(
                new ProductId(Product.NewID()),
                new ProductTitle("Title 1"),
                new ProductDescription("Description 1"),
                new ProductStatus(ProductStatusValue.Draft),
                new ProductPrice(200)
            ),
            new Product(
                new ProductId(Product.NewID()),
                new ProductTitle("Title 2"),
                new ProductDescription("Description 2"),
                new ProductStatus(ProductStatusValue.Draft),
                new ProductPrice(700)
            ),
        };

        products.ForEach(product =>
        {
            {
                string sql = "INSERT INTO ecommerce.product (id, title, description, price, status) VALUES (@Id, @Title, @Description, @Price, @Status)";
                var parameters = new DynamicParameters();
                parameters.Add("Id", product.Id);
                parameters.Add("Title", product.Title);
                parameters.Add("Description", product.Description);
                parameters.Add("Price", product.Price);
                parameters.Add("Status", product.Status);
                var command = new CommandDefinition(sql, parameters);
                conn.Execute(command);
            }

            {
                string sql = "INSERT INTO ecommerce.event (id, name, owner) VALUES (@Id, @Name, @Owner)";
                var parameters = new DynamicParameters();
                parameters.Add("Id", Product.NewID());
                parameters.Add("Name", "ecommerce_store_product_created");
                parameters.Add("Owner", product.Id);
                var command = new CommandDefinition(sql, parameters);
                conn.Execute(command);
            }
        });
    }
}
