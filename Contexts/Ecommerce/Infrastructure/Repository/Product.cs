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

    public async Task<Result<IEnumerable<Product>>> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
            await conn.OpenAsync(cancellationToken);

            const string sql = @"
                SELECT * FROM product
            ";

            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            var result = await conn.QueryAsync<ProductPrimitives>(command);
            if (result is null)
            {
                return new Result<IEnumerable<Product>> { Ok = Enumerable.Empty<Product>() };
            }

            var products = new List<Product>();

            foreach (var item in result)
            {
                Product product;
                try
                {
                    product = new Product
                    {
                        Id = new ProductId(item.Id),
                        Title = new ProductTitle(item.Title),
                        Description = new ProductDescription(item.Description),
                        Status = new ProductStatus(item.Status),
                        Price = new ProductPrice(item.Price)
                    };
                }
                catch (ProductError ex)
                {
                    return new Result<IEnumerable<Product>> { Err = (IError)ex };
                }

                product.AddTimeStamp(item.updated_at, item.created_at);

                products.Add(product);
            }

            return new Result<IEnumerable<Product>> { Ok = products };
        }
        catch (Exception)
        {
            return new Result<IEnumerable<Product>> { Err = new ProductPersistenceError() };
        }
    }

    public async Task<Result<Product>> GetById(Guid id, CancellationToken cancellationToken)
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
                return new Result<Product> { Err = new ProductNotFoundError() };
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
                return new Result<Product> { Err = (IError)ex };
            }

            product.AddTimeStamp(result.updated_at, result.created_at);

            return new Result<Product> { Ok = product };
        }
        catch (Exception)
        {
            return new Result<Product> { Err = new ProductPersistenceError() };
        }
    }

    public async Task<Result<bool>> Save(Product product, CancellationToken cancellationToken)
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

            return new Result<bool> { };
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
                                return new Result<bool> { Err = new ProductTitleUniqueError() };
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result<bool> { Err = new ProductTitleInvalidError() };

                                case ProductConstraints.CheckDescription:
                                    return new Result<bool> { Err = new ProductDescriptionInvalidError() };

                                case ProductConstraints.CheckStatus:
                                    return new Result<bool> { Err = new ProductStatusInvalidError() };

                                case ProductConstraints.CheckPrice:
                                    return new Result<bool> { Err = new ProductPriceInvalidError() };

                                default:
                                    break;
                            }

                            break;
                        }

                    default:
                        break;
                }
            }

            return new Result<bool> { Err = new ProductPersistenceError() };
        }
    }

    public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken)
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
                return new Result<bool> { Err = new ProductNotFoundError() };
            }

            return new Result<bool> { };
        }
        catch (Exception)
        {
            return new Result<bool> { Err = new ProductPersistenceError() };
        }
    }

    public async Task<Result<bool>> Update(Product product, CancellationToken cancellationToken)
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

            return new Result<bool> { };
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
                                return new Result<bool> { Err = new ProductTitleUniqueError() };
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result<bool> { Err = new ProductTitleInvalidError() };

                                case ProductConstraints.CheckDescription:
                                    return new Result<bool> { Err = new ProductDescriptionInvalidError() };

                                case ProductConstraints.CheckStatus:
                                    return new Result<bool> { Err = new ProductStatusInvalidError() };

                                case ProductConstraints.CheckPrice:
                                    return new Result<bool> { Err = new ProductPriceInvalidError() };

                                default:
                                    break;
                            }

                            break;
                        }

                    default:
                        break;
                }
            }

            return new Result<bool> { Err = new ProductPersistenceError() };
        }
    }
}
