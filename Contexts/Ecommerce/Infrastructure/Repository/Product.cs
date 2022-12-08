namespace Contexts.Ecommerce.Infrastructure.Repository;

using Contexts.Ecommerce.Domain.Entity;
using Contexts.Ecommerce.Domain.Exceptions;
using Contexts.Ecommerce.Domain.Model;
using Contexts.Ecommerce.Domain.Repository;
using Contexts.Ecommerce.Domain.ValueObject;
using Contexts.Ecommerce.Infrastructure.DataTransfer;
using Contexts.Ecommerce.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly DbContext _db;

    public ProductRepository(DbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<IEnumerable<Product>> Get(CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        var command = new CommandDefinition("SELECT * FROM ecommerce.product", cancellationToken: cancellationToken);

        var result = await conn.QueryAsync<ProductPrimitives>(command).ConfigureAwait(false);
        if (result is null)
        {
            return Enumerable.Empty<Product>();
        }

        Func<ProductPrimitives, Product> productsSelector = p =>
        {
            var product = new Product(
                new ProductId(p.Id),
                new ProductTitle(p.Title),
                new ProductDescription(p.Description),
                new ProductStatus((ProductStatusValue)p.Status),
                new ProductPrice(p.Price));

            product.AddTimeStamp(p.updated_at, p.created_at);

            return product;
        };

        return result.Select(productsSelector);
    }

    public async Task<IEnumerable<ProductEvent>> GetEvents(CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        string sql = @"
                SELECT *
                FROM ecommerce.event
                WHERE to_tsvector(name) @@ to_tsquery('ecommerce_store_product')
            ";

        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

        var result = await conn.QueryAsync<ProductEventPrimitives>(command).ConfigureAwait(false);
        if (result is null)
        {
            return Enumerable.Empty<ProductEvent>();
        }

        Func<ProductEventPrimitives, ProductEvent> selector = ev =>
        {
            var productNotification = new ProductEvent(
                new ProductEventId(ev.Id),
                new ProductId(ev.Owner),
                new ProductEventName(ev.Name));

            productNotification.AddTimeStamp(ev.updated_at, ev.created_at);

            return productNotification;
        };

        return result.Select(selector);
    }

    public async Task<Product> GetById(Guid id, CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        string sql = @"SELECT * FROM ecommerce.product WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        var result = await conn.QueryFirstOrDefaultAsync<ProductPrimitives>(command).ConfigureAwait(false);
        if (result is null)
        {
            throw new ProductNotFoundException();
        }

        var product = new Product(
            new ProductId(result.Id),
            new ProductTitle(result.Title),
            new ProductDescription(result.Description),
            new ProductStatus((ProductStatusValue)result.Status),
            new ProductPrice(result.Price));

        product.AddTimeStamp(result.updated_at, result.created_at);

        return product;
    }

    public async Task Save(Product product, CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        string sql = @"
                INSERT INTO ecommerce.product (id, title, description, price, status)
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

    public async Task SaveEvent(ProductEvent ev, CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        string sql = @"
                INSERT INTO ecommerce.event (id, name, owner)
                VALUES (@Id, @Name, @Owner)
            ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", ev.Id);
        parameters.Add("Name", ev.Name);
        parameters.Add("Owner", ev.Product);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        await conn.ExecuteAsync(command).ConfigureAwait(false);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        string sql = @"DELETE FROM ecommerce.product WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        int result = await conn.ExecuteAsync(command).ConfigureAwait(false);
        if (result is 0)
        {
            throw new ProductNotFoundException();
        }
    }

    public async Task Update(Product product, CancellationToken cancellationToken)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        string sql = @"
            UPDATE ecommerce.product
            SET title = @Title, description = @Description, price = @Price, status = @Status
            WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", product.Id);
        parameters.Add("Title", product.Title);
        parameters.Add("Description", product.Description);
        parameters.Add("Price", product.Price);
        parameters.Add("Status", product.Status);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        await conn.ExecuteAsync(command).ConfigureAwait(false);
    }
}
