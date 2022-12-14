namespace Ecommerce.Infrastructure.Repository;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exception;
using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.Persistence;

public sealed class ProductRepository : IProductRepository
{
    private string _connectionString { get; }

    public ProductRepository(IDbContext dbContext)
    {
        _connectionString = dbContext.GetConnectionString();
    }

    public async Task<OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT id, title, description, status, price
                FROM product
            ";

            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            var results = await conn.QueryAsync<ProductPrimitives>(command);

            return results is null
                ? OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>.FromT0(Enumerable.Empty<ProductPrimitives>())
                : OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>.FromT0(results);
        }
        catch (Exception ex)
        {
            return HandleRepositoryOperationException(ex);
        }
    }

    public async Task<OneOf<ProductPrimitives, ProblemDetailsException>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT id, title, description, status, price FROM product
                WHERE id = @Id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            var result = await conn.QueryFirstOrDefaultAsync<ProductPrimitives>(command);
            if (result.Id == default)
            {
                return new ProductNotFoundException();
            }

            return result;
        }
        catch (Exception ex)
        {
            return HandleRepositoryOperationException(ex);
        }
    }

    public async Task<OneOf<byte, ProblemDetailsException>> Save(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                INSERT INTO product (id, title, description, price, status)
                VALUES (@Id, @Title, @Description, @Price, @Status::product_status)
            ";

            var primitives = product.ToPrimitives();

            var parameters = new DynamicParameters();
            parameters.Add("Id", primitives.Id);
            parameters.Add("Title", primitives.Title);
            parameters.Add("Description", primitives.Description);
            parameters.Add("Price", primitives.Price);
            parameters.Add("Status", primitives.Status);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            await conn.ExecuteAsync(command);

            return default;
        }
        catch (Exception ex)
        {
            return HandleRepositoryOperationException(ex);
        }
    }

    public async Task<OneOf<byte, ProblemDetailsException>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                DELETE FROM product
                WHERE id = @Id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            var result = await conn.ExecuteAsync(command);

            return result is DbConstants.NotAffectedRows ? new ProductNotFoundException() : default(byte);
        }
        catch (Exception ex)
        {
            return HandleRepositoryOperationException(ex);
        }
    }

    public async Task<OneOf<byte, ProblemDetailsException>> Update(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                UPDATE product
                    SET title = @Title,
                        description = @Description,
                        price = @Price,
                        status = @Status::product_status
                WHERE id = @Id
            ";

            var primitives = product.ToPrimitives();

            var parameters = new DynamicParameters();
            parameters.Add("Id", primitives.Id);
            parameters.Add("Title", primitives.Title);
            parameters.Add("Description", primitives.Description);
            parameters.Add("Price", primitives.Price);
            parameters.Add("Status", primitives.Status);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            await conn.ExecuteAsync(command);

            return default;
        }
        catch (Exception ex)
        {
            return HandleRepositoryOperationException(ex);
        }
    }

    private static ProblemDetailsException HandleRepositoryOperationException(Exception ex)
    {
        if (ex is not PostgresException postgresException)
        {
            return new ProductPersistenceException(ex.Message);
        }

        switch (postgresException.SqlState)
        {
            case PostgresErrorCodes.UniqueViolation:
            {
                switch (postgresException.ConstraintName)
                {
                    case ProductConstraints.UniqueId:
                        return new ProductIdUniqueException();

                    case ProductConstraints.UniqueTitle:
                        return new ProductTitleUniqueException();
                }

                break;
            }
        }

        return new ProductPersistenceException(postgresException.MessageText);
    }
}