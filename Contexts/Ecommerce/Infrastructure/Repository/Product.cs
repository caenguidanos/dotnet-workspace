namespace Ecommerce.Infrastructure.Repository;

using Dapper;
using Npgsql;
using Common.Domain;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exception;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.DataTransfer;
using Ecommerce.Infrastructure.Persistence;

public sealed class ProductRepository : IProductRepository
{
    private IDbContext _dbContext { get; }

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<Product>, ProblemDetailsException>> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT id, title, description, status, price, __created_at__ CreatedAt, __updated_at__ UpdatedAt
                FROM product
            ";

            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            var results = await conn.QueryAsync<ProductPrimitives>(command);
            if (results is null)
            {
                return new Result<IEnumerable<Product>, ProblemDetailsException>(Enumerable.Empty<Product>());
            }

            var products = new List<Product>();

            foreach (var result in results)
            {
                var product = new Product
                {
                    Id = new ProductId(result.Id),
                    Title = new ProductTitle(result.Title),
                    Description = new ProductDescription(result.Description),
                    Status = new ProductStatus(result.Status),
                    Price = new ProductPrice(result.Price)
                };

                product.AddTimeStamp(createdAt: result.CreatedAt, updatedAt: result.UpdatedAt);

                var productIntegrityResult = product.CheckIntegrity();
                if (productIntegrityResult.IsFaulted)
                {
                    return new Result<IEnumerable<Product>, ProblemDetailsException>(productIntegrityResult.Error);
                }

                products.Add(product);
            }

            return new Result<IEnumerable<Product>, ProblemDetailsException>(products);
        }
        catch (Exception ex)
        {
            return new Result<IEnumerable<Product>, ProblemDetailsException>(new ProductPersistenceException(ex.Message));
        }
    }

    public async Task<Result<Product, ProblemDetailsException>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT id, title, description, status, price, __created_at__ CreatedAt, __updated_at__ UpdatedAt
                FROM product
                WHERE id = @Id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            var result = await conn.QueryFirstOrDefaultAsync<ProductPrimitives>(command);
            if (result is null)
            {
                return new Result<Product, ProblemDetailsException>(new ProductNotFoundException());
            }

            var product = new Product
            {
                Id = new ProductId(result.Id),
                Title = new ProductTitle(result.Title),
                Description = new ProductDescription(result.Description),
                Status = new ProductStatus(result.Status),
                Price = new ProductPrice(result.Price)
            };

            product.AddTimeStamp(createdAt: result.CreatedAt, updatedAt: result.UpdatedAt);

            var productIntegrityResult = product.CheckIntegrity();

            return productIntegrityResult.IsFaulted
                ? new Result<Product, ProblemDetailsException>(productIntegrityResult.Error)
                : new Result<Product, ProblemDetailsException>(product);
        }
        catch (Exception ex)
        {
            return new Result<Product, ProblemDetailsException>(new ProductPersistenceException(ex.Message));
        }
    }

    public async Task<Result<ResultUnit, ProblemDetailsException>> Save(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                INSERT INTO product (id, title, description, price, status)
                VALUES (@Id, @Title, @Description, @Price, @Status)
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

            return new Result<ResultUnit, ProblemDetailsException>();
        }
        catch (Exception ex)
        {
            if (ex is not PostgresException postgresException)
            {
                return new Result<ResultUnit, ProblemDetailsException>(new ProductPersistenceException(ex.Message));
            }

            switch (postgresException.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:
                {
                    if (postgresException.ConstraintName == ProductConstraints.UniqueTitle)
                    {
                        return new Result<ResultUnit, ProblemDetailsException>(new ProductTitleUniqueException());
                    }

                    break;
                }
            }

            return new Result<ResultUnit, ProblemDetailsException>(
                new ProductPersistenceException(postgresException.MessageText));
        }
    }

    public async Task<Result<ResultUnit, ProblemDetailsException>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                DELETE FROM product WHERE id = @Id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            var result = await conn.ExecuteAsync(command);

            return result is 0
                ? new Result<ResultUnit, ProblemDetailsException>(new ProductNotFoundException())
                : new Result<ResultUnit, ProblemDetailsException>();
        }
        catch (Exception ex)
        {
            return new Result<ResultUnit, ProblemDetailsException>(new ProductPersistenceException(ex.Message));
        }
    }

    public async Task<Result<ResultUnit, ProblemDetailsException>> Update(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                UPDATE product
                SET title = @Title, description = @Description, price = @Price, status = @Status
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

            return new Result<ResultUnit, ProblemDetailsException>();
        }
        catch (Exception ex)
        {
            if (ex is not PostgresException postgresException)
            {
                return new Result<ResultUnit, ProblemDetailsException>(new ProductPersistenceException(ex.Message));
            }

            switch (postgresException.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:
                {
                    if (postgresException.ConstraintName == ProductConstraints.UniqueTitle)
                    {
                        return new Result<ResultUnit, ProblemDetailsException>(new ProductTitleUniqueException());
                    }

                    break;
                }
            }

            return new Result<ResultUnit, ProblemDetailsException>(
                new ProductPersistenceException(postgresException.MessageText));
        }
    }
}