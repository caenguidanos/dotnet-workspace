namespace Common.Domain;

public interface IHttpError
{
    public int StatusCode { get; }
    public string Detail { get; }
}
