namespace Common.Domain;

public class Schema
{
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static Guid NewID()
    {
        return Guid.NewGuid();
    }

    public void AddTimeStamp(DateTime updatedAt, DateTime createdAt)
    {
        updated_at = updatedAt;
        created_at = createdAt;
    }
}

public class SchemaPrimitives
{
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}
