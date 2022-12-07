namespace Common.Domain.Entity;

public class TimeStamp
{
    public DateTime created_at { get; private set; }
    public DateTime updated_at { get; private set; }

    public void WithTimeStamp(DateTime updatedAt, DateTime createdAt)
    {
        updated_at = updatedAt;
        created_at = createdAt;
    }
}
