namespace Common.Domain;

public class Schema
{
    private readonly List<IError> _errors = new();

    public DateTime created_at { get; private set; }
    public DateTime updated_at { get; private set; }

    public static Guid NewID()
    {
        return Guid.NewGuid();
    }

    public void AddTimeStamp(DateTime updatedAt, DateTime createdAt)
    {
        updated_at = updatedAt;
        created_at = createdAt;
    }

    public void AddError(IError error)
    {
        _errors.Add(error);
    }

    public bool HasError()
    {
        Validate();
        return _errors.Count > 0;
    }

    public IError GetError()
    {
        return _errors.First();
    }

    protected virtual void Validate()
    {
        throw new NotImplementedException();
    }
}

public class SchemaPrimitives
{
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}
