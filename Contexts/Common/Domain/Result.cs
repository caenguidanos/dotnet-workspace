namespace Common.Domain;

public enum ResultState : byte
{
    Faulted,
    Success
}

public readonly struct Result<A, E>
{
    internal readonly ResultState State;
    public readonly A Ok;
    public readonly E Err;

    public Result(A value)
    {
        State = ResultState.Success;
        Ok = value;
        Err = default!;
    }

    public Result(E e)
    {
        State = ResultState.Faulted;
        Err = e;
        Ok = default!;
    }

    public bool IsFaulted
    {
        get
        {
            return State == ResultState.Faulted;
        }
    }

    public R Match<R>(Func<A, R> Succ, Func<E, R> Fail)
    {
        return IsFaulted
                ? Fail(Err)
                : Succ(Ok);
    }

    public async Task<R> MatchAsync<R>(Func<A, Task<R>> Succ, Func<E, Task<R>> Fail)
    {
        return IsFaulted
            ? await Fail(Err)
            : await Succ(Ok);
    }
}
