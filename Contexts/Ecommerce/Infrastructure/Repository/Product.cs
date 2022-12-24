namespace Ecommerce.Infrastructure;

public sealed class ProductRepository : IProductRepository
{
    private IDbContext _dbContext { get; }

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<List<Product>, ProblemDetailsException>> Get(CancellationToken cancellationToken)
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

            var products = new List<Product>();

            var results = await conn.QueryAsync<ProductPrimitives>(command);
            if (results is null) return products;

            foreach (var result in results)
            {
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

                    product.AddTimeStamp(createdAt: result.CreatedAt, updatedAt: result.UpdatedAt);
                }
                catch (ProblemDetailsException ex)
                {
                    return ex;
                }

                products.Add(product);
            }

            return products;
        }
        catch (Exception ex)
        {
            return new ProductPersistenceException(ex.Message);
        }
    }

    public async Task<OneOf<Product, ProblemDetailsException>> GetById(Guid id, CancellationToken cancellationToken)
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
            if (result is null) return new ProductNotFoundException();

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

                product.AddTimeStamp(createdAt: result.CreatedAt, updatedAt: result.UpdatedAt);
            }
            catch (ProblemDetailsException ex)
            {
                return ex;
            }

            return product;
        }
        catch (Exception ex)
        {
            return new ProductPersistenceException(ex.Message);
        }
    }

    public async Task<OneOf<byte, ProblemDetailsException>> Save(Product product, CancellationToken cancellationToken)
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

            return default;
        }
        catch (Exception ex)
        {
            if (ex is not PostgresException postgresException)
            {
                return new ProductPersistenceException(ex.Message);
            }

            switch (postgresException.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:
                {
                    if (postgresException.ConstraintName == ProductConstraints.UniqueTitle)
                    {
                        return new ProductTitleUniqueException();
                    }

                    break;
                }
            }

            return new ProductPersistenceException(postgresException.MessageText);
        }
    }

    public async Task<OneOf<byte, ProblemDetailsException>> Delete(Guid id, CancellationToken cancellationToken)
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

            return result is 0 ? new ProductNotFoundException() : default(byte);
        }
        catch (Exception ex)
        {
            return new ProductPersistenceException(ex.Message);
        }
    }

    public async Task<OneOf<byte, ProblemDetailsException>> Update(Product product, CancellationToken cancellationToken)
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

            return default;
        }
        catch (Exception ex)
        {
            if (ex is not PostgresException postgresException)
            {
                return new ProductPersistenceException(ex.Message);
            }

            switch (postgresException.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:
                {
                    if (postgresException.ConstraintName == ProductConstraints.UniqueTitle)
                    {
                        return new ProductTitleUniqueException();
                    }

                    break;
                }
            }

            return new ProductPersistenceException(postgresException.MessageText);
        }
    }
}