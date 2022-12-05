namespace Ecommerce.Store.Infrastructure.Repository;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.ValueObject;
using Ecommerce.Store.Infrastructure.Environment;
using Ecommerce.Store.Infrastructure.Model;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(ConfigurationSettings configuration)
    {
        _connectionString = configuration.PostgresConnection;
    }

    public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        var command = new CommandDefinition("SELECT * FROM product", cancellationToken: cancellationToken);

        var result = await conn.QueryAsync<ProductPrimitives>(command).ConfigureAwait(false);
        if (result is null)
        {
            return Enumerable.Empty<Product>();
        }

        Func<ProductPrimitives, Product> productsSelector = product =>
        {
            return new Product(
                new ProductId(product.Id),
                new ProductTitle(product.Title),
                new ProductDescription(product.Description),
                new ProductStatus((ProductStatusValue)product.Status),
                new ProductPrice(product.Price));
        };

        return result.Select(productsSelector);
    }

    public async Task<Product> GetById(Guid id, CancellationToken cancellationToken)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        string sql = @"SELECT * FROM product WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        var result = await conn.QueryFirstOrDefaultAsync<ProductPrimitives>(command).ConfigureAwait(false);
        if (result is null)
        {
            throw new ProductNotFoundException();
        }

        return new Product(
            new ProductId(result.Id),
            new ProductTitle(result.Title),
            new ProductDescription(result.Description),
            new ProductStatus((ProductStatusValue)result.Status),
            new ProductPrice(result.Price));
    }

    public async Task Save(Product product, CancellationToken cancellationToken)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        string sql = @"
                INSERT INTO product (id, title, description, price, status)
                VALUES (@Id, @Title, @Description, @Price, @Status)
            ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", product.Id);
        parameters.Add("Title", product.Title);
        parameters.Add("Description", product.Description);
        parameters.Add("Price", product.Price);
        parameters.Add("Status", product.Status);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        await conn.ExecuteAsync(command).ConfigureAwait(false);
    }

    public async Task DeleteById(Guid id, CancellationToken cancellationToken)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        string sql = @"DELETE FROM product WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        int result = await conn.ExecuteAsync(command).ConfigureAwait(false);
        if (result is 0)
        {
            throw new ProductNotFoundException();
        }
    }
}
