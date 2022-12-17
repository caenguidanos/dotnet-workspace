namespace Common.Domain;

using System.Globalization;

public record Schema<TEntity, TPrimitives>
{
    protected static readonly CultureInfo locale = new("en-US");

    public DateTime created_at { get; private set; }
    public DateTime updated_at { get; private set; }

    public Guid NewID()
    {
        return Guid.NewGuid();
    }

    public void AddTimeStamp(DateTime updatedAt, DateTime createdAt)
    {
        updated_at = updatedAt;
        created_at = createdAt;
    }

    public virtual TPrimitives ToPrimitives()
    {
        throw new NotImplementedException();
    }
}

public record SchemaPrimitives
{
    public required DateTime created_at { get; set; }
    public required DateTime updated_at { get; set; }
}
