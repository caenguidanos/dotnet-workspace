namespace Common.Domain;

using System.Text.Json.Serialization;

public record Schema<TPrimitives>
{
    public DateTime CreatedAd { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void AddTimeStamp(DateTime createdAt, DateTime updatedAt)
    {
        CreatedAd = createdAt;
        UpdatedAt = updatedAt;
    }

    public virtual TPrimitives ToPrimitives() => throw new NotImplementedException();
}

public record SchemaPrimitives
{
    [JsonPropertyName("created_at")] public required DateTime CreatedAt { get; init; }
    [JsonPropertyName("updated_at")] public required DateTime UpdatedAt { get; init; }
}