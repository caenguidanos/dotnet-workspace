namespace Common.Domain.Service;

public interface ILoggerService
{
    public void Print(int ev, string msg)
    {
        Console.WriteLine(@"Log[{ev}]: {msg}");
    }
}
