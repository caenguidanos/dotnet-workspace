namespace Common.Domain;

public enum ResultState : byte
{
    Faulted,
    Success
}

public readonly struct ResultUnit
{
}

public readonly struct Result<TSuccess, TError>
{
    private readonly ResultState _state;

    public readonly TSuccess Value = default!;
    public readonly TError Error = default!;

    // For responses that not have any returned payload.
    public Result()
    {
        _state = ResultState.Success;
    }

    public Result(TSuccess value)
    {
        _state = ResultState.Success;
        Value = value;
    }

    public Result(TError value)
    {
        _state = ResultState.Faulted;
        Error = value;
    }

    public bool IsFaulted => _state == ResultState.Faulted;

    public TResult Match<TResult>(Func<TSuccess, TResult> success, Func<TError, TResult> fail)
    {
        return IsFaulted
            ? fail(Error)
            : success(Value);
    }
}