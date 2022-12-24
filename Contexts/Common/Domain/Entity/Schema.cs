namespace Common.Domain;

public record Schema<T>
{
    public DateTime CreatedAd { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void AddTimeStamp(DateTime createdAt, DateTime updatedAt)
    {
        CreatedAd = ValidateTimeStamp(createdAt);
        UpdatedAt = ValidateTimeStamp(updatedAt);
    }

    public virtual T ToPrimitives() => throw new NotImplementedException();

    private static DateTime ValidateTimeStamp(DateTime candidate)
    {
        if (candidate == default)
        {
            throw new TimeStampException();
        }

        return candidate;
    }
}

public record SchemaPrimitives
{
    [JsonPropertyName("created_at")] public required DateTime CreatedAt { get; init; }
    [JsonPropertyName("updated_at")] public required DateTime UpdatedAt { get; init; }
}