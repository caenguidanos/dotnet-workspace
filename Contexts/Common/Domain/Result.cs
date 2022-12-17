namespace Common.Domain;

public enum ResultState : byte
{
    Faulted,
    Success
}

public readonly struct Result<TSuccess, TError>
{
    internal readonly ResultState State;

    public readonly TSuccess Value = default!;
    public readonly TError Error = default!;

    // for responses that not have any returned paylaod
    public Result()
    {
        State = ResultState.Success;
    }

    public Result(TSuccess value)
    {
        State = ResultState.Success;
        Value = value;
    }

    public Result(TError value)
    {
        State = ResultState.Faulted;
        Error = value;
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
