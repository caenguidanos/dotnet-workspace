namespace Ecommerce.AcceptanceTest.Infrastructure.Persistence;

using Dapper;
using Moq;
using Npgsql;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Infrastructure.Persistence;

public sealed class ProductTableAcceptanceTest
{
    private readonly PostgresDatabaseFactory _postgres = new(template: "ecommerce");

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        string connectionString = await _postgres.StartAsync();

        Mock
            .Get(_dbContext)
            .Setup(dbContext => dbContext.GetConnectionString())
            .Returns(connectionString);
    }

    [Test]
    public async Task GivenIdDefinition_WhenInsert_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);
    }

    [Test]
    public async Task GivenTitleDefinition_WhenInsert_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);
    }

    [Test]
    public async Task GivenTitleDefinition_WhenInsertWithInvalidLength_I_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', '1234', 'Great guitar', 219900, 1);
        ";

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckTitle));
        }
    }

    [Test]
    public async Task GivenTitleDefinition_WhenInsertWithInvalidLength_II_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string title = string.Empty;
        for (int i = 0; i < 257; i++)
        {
            title += "a";
        }

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', @Title, 'Great guitar', 219900, 1);
        ";

        var paramters = new DynamicParameters();
        paramters.Add("Title", title);

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql, paramters));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckTitle));
        }
    }

    [Test]
    public async Task GivenDescriptionDefinition_WhenInsert_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);
    }

    [Test]
    public async Task GivenDescriptionDefinition_WhenInsertWithInvalidLength_I_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', '1234', 219900, 1);
        ";

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckDescription));
        }
    }

    [Test]
    public async Task GivenDescriptionDefinition_WhenInsertWithInvalidLength_II_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string description = string.Empty;
        for (int i = 0; i < 601; i++)
        {
            description += "a";
        }

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', @Description, 219900, 1);
        ";

        var paramters = new DynamicParameters();
        paramters.Add("Description", description);

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql, paramters));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckDescription));
        }
    }

    [Test]
    public async Task GivenPriceDefinition_WhenInsert_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);
    }

    [Test]
    public async Task GivenPriceDefinition_WhenInsertWithInvalidSize_I_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 1, 1);
        ";

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckPrice));
        }
    }

    [Test]
    public async Task GivenPriceDefinition_WhenInsertWithInvalidSize_II_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 100000001, 1);
        ";

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckPrice));
        }
    }

    [Test]
    public async Task GivenStatusDefinition_WhenInsertClosed_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 0);
        ";

        await conn.ExecuteAsync(sql);
    }

    [Test]
    public async Task GivenStatusDefinition_WhenInsertPublished_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);
    }

    [Test]
    public async Task GivenStatusDefinition_WhenInsertWithInvalidValue_ThenThrowException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('5436a253-25d5-4315-b985-01a671874acb', 'American Professional II Stratocaster', 'Great guitar', 219900, 98);
        ";

        var exception = Assert.ThrowsAsync<PostgresException>(
            async () => await conn.ExecuteAsync(sql));

        if (exception is not null)
        {
            Assert.That(exception.SqlState, Is.EqualTo(PostgresErrorCodes.CheckViolation));
            Assert.That(exception.ConstraintName, Is.EqualTo(ProductConstraints.CheckStatus));
        }
    }
}
