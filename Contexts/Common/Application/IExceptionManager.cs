namespace Common.Application;

public interface IExceptionManager
{
    public HttpResultResponse HandleHttp(Exception ex);
}