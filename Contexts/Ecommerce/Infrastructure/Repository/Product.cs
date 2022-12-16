namespace Ecommerce.Infrastructure.Repository;

using Dapper;
using Npgsql;

using Common.Domain;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Error;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.DataTransfer;
using Ecommerce.Infrastructure.Persistence;

public sealed class ProductRepository : IProductRepository
{
    private IDbContext _dbContext { get; init; }

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<Product>, ProductError>> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT * FROM product
            ";

            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            var results = await conn.QueryAsync<ProductPrimitives>(command);
            if (results is null)
            {
                return new Result<IEnumerable<Product>, ProductError>(Enumerable.Empty<Product>());
            }

            var products = results.Select(productPrimitives =>
                {
                    var product = new Product
                    {
                        Id = new ProductId(productPrimitives.Id),
                        Title = new ProductTitle(productPrimitives.Title),
                        Description = new ProductDescription(productPrimitives.Description),
                        Status = new ProductStatus(productPrimitives.Status),
                        Price = new ProductPrice(productPrimitives.Price)
                    };

                    product.AddTimeStamp(productPrimitives.updated_at, productPrimitives.created_at);

                    return product;
                });

            return new Result<IEnumerable<Product>, ProductError>(products);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<Product>, ProductError>(new ProductPersistenceError());
        }
    }

    public async Task<Result<Product, ProductError>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT * FROM product WHERE id = @Id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

            var result = await conn.QueryFirstOrDefaultAsync<ProductPrimitives>(command);
            if (result is null)
            {
                return new Result<Product, ProductError>(new ProductNotFoundError());
            }

            Product product;
            try
            {
                product = new Product
                {
                    Id = new ProductId(result.Id),
                    Title = new ProductTitle(result.Title),
                    Description = new ProductDescription(result.Description),
                    Status = new ProductStatus(result.Status),
                    Price = new ProductPrice(result.Price)
                };
            }
            catch (ProductError ex)
            {
                return new Result<Product, ProductError>(ex);
            }

            product.AddTimeStamp(result.updated_at, result.created_at);

            return new Result<Product, ProductError>(product);
        }
        catch (Exception)
        {
            return new Result<Product, ProductError>(new ProductPersistenceError());
        }
    }

    public async Task<Result<byte, ProductError>> Save(Product product, CancellationToken cancellationToken)
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

            return new Result<byte, ProductError>();
        }
        catch (Exception ex)
        {
            if (ex is PostgresException postgresException)
            {
                switch (postgresException.SqlState)
                {
                    case PostgresErrorCodes.UniqueViolation:
                        {
                            if (postgresException.ConstraintName == ProductConstraints.UniqueTitle)
                            {
                                return new Result<byte, ProductError>(new ProductTitleUniqueError());
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result<byte, ProductError>(new ProductTitleInvalidError());

                                case ProductConstraints.CheckDescription:
                                    return new Result<byte, ProductError>(new ProductDescriptionInvalidError());

                                case ProductConstraints.CheckStatus:
                                    return new Result<byte, ProductError>(new ProductStatusInvalidError());

                                case ProductConstraints.CheckPrice:
                                    return new Result<byte, ProductError>(new ProductPriceInvalidError());

                                default:
                                    break;
                            }

                            break;
                        }

                    default:
                        break;
                }
            }

            return new Result<byte, ProductError>(new ProductPersistenceError());
        }
    }

    public async Task<Result<byte, ProductError>> Delete(Guid id, CancellationToken cancellationToken)
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

            int result = await conn.ExecuteAsync(command);
            if (result is 0)
            {
                return new Result<byte, ProductError>(new ProductNotFoundError());
            }

            return new Result<byte, ProductError>();
        }
        catch (Exception)
        {
            return new Result<byte, ProductError>(new ProductPersistenceError());
        }
    }

    public async Task<Result<byte, ProductError>> Update(Product product, CancellationToken cancellationToken)
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

            return new Result<byte, ProductError>();
        }
        catch (Exception ex)
        {
            if (ex is PostgresException postgresException)
            {
                switch (postgresException.SqlState)
                {
                    case PostgresErrorCodes.UniqueViolation:
                        {
                            if (postgresException.ConstraintName == ProductConstraints.UniqueTitle)
                            {
                                return new Result<byte, ProductError>(new ProductTitleUniqueError());
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result<byte, ProductError>(new ProductTitleInvalidError());

                                case ProductConstraints.CheckDescription:
                                    return new Result<byte, ProductError>(new ProductDescriptionInvalidError());

                                case ProductConstraints.CheckStatus:
                                    return new Result<byte, ProductError>(new ProductStatusInvalidError());

                                case ProductConstraints.CheckPrice:
                                    return new Result<byte, ProductError>(new ProductPriceInvalidError());

                                default:
                                    break;
                            }

                            break;
                        }

                    default:
                        break;
                }
            }

            return new Result<byte, ProductError>(new ProductPersistenceError());
        }
    }
}
