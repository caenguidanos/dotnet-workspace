namespace Common.Domain;

public enum ResultState : byte
{
    Faulted,
    Success
}

public readonly struct Result<TSuccess, TError>
{
    internal readonly ResultState State;

    public readonly TSuccess Value;
    public readonly TError Error;

    public Result(TSuccess value)
    {
        State = ResultState.Success;
        Value = value;
        Error = default!;
    }

    public Result(TError e)
    {
        State = ResultState.Faulted;
        Error = e;
        Value = default!;
    }

    public bool IsFaulted
    {
        get
        {
            return State == ResultState.Faulted;
        }
    }

    public TResult Match<TResult>(Func<TSuccess, TResult> Succ, Func<TError, TResult> Fail)
    {
        return IsFaulted
                ? Fail(Error)
                : Succ(Value);
    }
}
