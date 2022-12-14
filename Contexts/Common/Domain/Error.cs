namespace Common.Domain;

public interface IError
{
    public int StatusCode { get; }
    public string Detail { get; }
}
