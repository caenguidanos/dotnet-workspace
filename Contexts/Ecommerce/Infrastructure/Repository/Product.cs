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

    public async Task<Result<IEnumerable<Product>, ProductException>> Get(CancellationToken cancellationToken)
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
                return new Result<IEnumerable<Product>, ProductException>(Enumerable.Empty<Product>());
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

                product.AddTimeStamp(result.updated_at, result.created_at);

                var productIntegrityResult = product.CheckIntegrity();
                if (productIntegrityResult.IsFaulted)
                {
                    return new Result<IEnumerable<Product>, ProductException>(productIntegrityResult.Error);
                }

                products.Add(product);
            }

            return new Result<IEnumerable<Product>, ProductException>(products);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<Product>, ProductException>(new ProductPersistenceException());
        }
    }

    public async Task<Result<Product, ProductException>> GetById(Guid id, CancellationToken cancellationToken)
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
                return new Result<Product, ProductException>(new ProductNotFoundException());
            }

            var product = new Product
            {
                Id = new ProductId(result.Id),
                Title = new ProductTitle(result.Title),
                Description = new ProductDescription(result.Description),
                Status = new ProductStatus(result.Status),
                Price = new ProductPrice(result.Price)
            };

            product.AddTimeStamp(result.updated_at, result.created_at);

            var productIntegrityResult = product.CheckIntegrity();
            if (productIntegrityResult.IsFaulted)
            {
                return new Result<Product, ProductException>(productIntegrityResult.Error);
            }

            return new Result<Product, ProductException>(product);
        }
        catch (Exception)
        {
            return new Result<Product, ProductException>(new ProductPersistenceException());
        }
    }

    public async Task<Result<byte, ProductException>> Save(Product product, CancellationToken cancellationToken)
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

            return new Result<byte, ProductException>();
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
                                return new Result<byte, ProductException>(new ProductTitleUniqueException());
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result<byte, ProductException>(new ProductTitleInvalidException());

                                case ProductConstraints.CheckDescription:
                                    return new Result<byte, ProductException>(new ProductDescriptionInvalidException());

                                case ProductConstraints.CheckStatus:
                                    return new Result<byte, ProductException>(new ProductStatusInvalidException());

                                case ProductConstraints.CheckPrice:
                                    return new Result<byte, ProductException>(new ProductPriceInvalidException());
                            }

                            break;
                        }
                }
            }

            return new Result<byte, ProductException>(new ProductPersistenceException());
        }
    }

    public async Task<Result<byte, ProductException>> Delete(Guid id, CancellationToken cancellationToken)
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
                return new Result<byte, ProductException>(new ProductNotFoundException());
            }

            return new Result<byte, ProductException>();
        }
        catch (Exception)
        {
            return new Result<byte, ProductException>(new ProductPersistenceException());
        }
    }

    public async Task<Result<byte, ProductException>> Update(Product product, CancellationToken cancellationToken)
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

            return new Result<byte, ProductException>();
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
                                return new Result<byte, ProductException>(new ProductTitleUniqueException());
                            }

                            break;
                        }
                    case PostgresErrorCodes.CheckViolation:
                        {
                            switch (postgresException.ConstraintName)
                            {
                                case ProductConstraints.CheckTitle:
                                    return new Result<byte, ProductException>(new ProductTitleInvalidException());

                                case ProductConstraints.CheckDescription:
                                    return new Result<byte, ProductException>(new ProductDescriptionInvalidException());

                                case ProductConstraints.CheckStatus:
                                    return new Result<byte, ProductException>(new ProductStatusInvalidException());

                                case ProductConstraints.CheckPrice:
                                    return new Result<byte, ProductException>(new ProductPriceInvalidException());
                            }

                            break;
                        }
                }
            }

            return new Result<byte, ProductException>(new ProductPersistenceException());
        }
    }
}
