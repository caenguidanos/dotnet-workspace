namespace Ecommerce.Infrastructure.Repository;

using Dapper;
using Npgsql;

using Common.Domain;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Error;
using Ecommerce.Domain.Repository;
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

            var result = await conn.QueryAsync<ProductPrimitives>(command).ConfigureAwait(false);
            if (result is null)
            {
                return new Result<IEnumerable<Product>>(Enumerable.Empty<Product>());
            }

            var products = new List<Product>();

            foreach (var item in result)
            {
                var product = new Product
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Status = item.Status,
                    Price = item.Price
                };

                if (product.HasError())
                {
                    return new Result<IEnumerable<Product>>(null, product.GetError());
                }

                product.AddTimeStamp(item.updated_at, item.created_at);

                products.Add(product);
            }

            return new Result<IEnumerable<Product>>(products);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<Product>>(null, new ProductPersistenceError());
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

            var result = await conn.QueryFirstOrDefaultAsync<ProductPrimitives>(command).ConfigureAwait(false);
            if (result is null)
            {
                return new Result<Product>(null, new ProductNotFoundError());
            }

            var product = new Product
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Status = result.Status,
                Price = result.Price
            };

            if (product.HasError())
            {
                return new Result<Product>(null, product.GetError());
            }

            product.AddTimeStamp(result.updated_at, result.created_at);

            return new Result<Product>(product);
        }
        catch (Exception)
        {
            return new Result<Product>(null, new ProductPersistenceError());
        }
    }

    public async Task<Result> Save(Product product, CancellationToken cancellationToken)
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

            await conn.ExecuteAsync(command).ConfigureAwait(false);

            return new Result();
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
                                return new Result(new ProductTitleUniqueError());
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result(new ProductTitleInvalidError());


                                case ProductConstraints.CheckDescription:
                                    return new Result(new ProductDescriptionInvalidError());


                                case ProductConstraints.CheckStatus:
                                    return new Result(new ProductStatusInvalidError());


                                case ProductConstraints.CheckPrice:
                                    return new Result(new ProductPriceInvalidError());

                                default:
                                    break;
                            }

                            break;
                        }

                    default:
                        break;
                }
            }

            return new Result(new ProductPersistenceError());
        }
    }

    public async Task<Result> Delete(Guid id, CancellationToken cancellationToken)
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

            int result = await conn.ExecuteAsync(command).ConfigureAwait(false);
            if (result is 0)
            {
                return new Result(new ProductNotFoundError());
            }

            return new Result();
        }
        catch (Exception)
        {
            return new Result(new ProductPersistenceError());
        }
    }

    public async Task<Result> Update(Product product, CancellationToken cancellationToken)
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

            await conn.ExecuteAsync(command).ConfigureAwait(false);

            return new Result();
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
                                return new Result(new ProductTitleUniqueError());
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result(new ProductTitleInvalidError());


                                case ProductConstraints.CheckDescription:
                                    return new Result(new ProductDescriptionInvalidError());


                                case ProductConstraints.CheckStatus:
                                    return new Result(new ProductStatusInvalidError());


                                case ProductConstraints.CheckPrice:
                                    return new Result(new ProductPriceInvalidError());

                                default:
                                    break;
                            }

                            break;
                        }

                    default:
                        break;
                }
            }

            return new Result(new ProductPersistenceError());
        }
    }
}
