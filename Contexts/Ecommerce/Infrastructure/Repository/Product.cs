namespace Ecommerce.Infrastructure.Repository;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.DataTransfer;
using Ecommerce.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly IDbContext _dbContext;

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            var command = new CommandDefinition("SELECT * FROM public.product", cancellationToken: cancellationToken);

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

            return result.Select(productsSelector).AsList();
        }
        catch (System.Exception exception)
        {
            if (exception is Npgsql.PostgresException)
            {
                throw new ProductRepositoryPersistenceException(exception.Message);
            }

            throw;
        }
    }

    public async Task<Product> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            string sql = @"SELECT * FROM public.product WHERE id = @Id";

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
        catch (System.Exception exception)
        {
            if (exception is Npgsql.PostgresException)
            {
                throw new ProductRepositoryPersistenceException(exception.Message);
            }

            throw;
        }
    }

    public async Task Save(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            string sql = @"
                INSERT INTO public.product (id, title, description, price, status)
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
        catch (System.Exception exception)
        {
            if (exception is Npgsql.PostgresException)
            {
                throw new ProductRepositoryPersistenceException(exception.Message);
            }

            throw;
        }
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            string sql = @"DELETE FROM public.product WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            int result = await conn.ExecuteAsync(command).ConfigureAwait(false);
            if (result is 0)
            {
                throw new ProductNotFoundException();
            }
        }
        catch (System.Exception exception)
        {
            if (exception is Npgsql.PostgresException)
            {
                throw new ProductRepositoryPersistenceException(exception.Message);
            }

            throw;
        }
    }

    public async Task Update(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            string sql = @"
            UPDATE public.product
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
        catch (System.Exception exception)
        {
            if (exception is Npgsql.PostgresException)
            {
                throw new ProductRepositoryPersistenceException(exception.Message);
            }

            throw;
        }
    }
}
