namespace Common.Domain;

public readonly struct Result
{
    public readonly IError? Err;

    public Result(IError? err = null)
    {
        Err = err;
    }
}

public readonly struct Result<TResult>
{
    public readonly TResult? Ok;
    public readonly IError? Err;

    public Result(TResult? ok, IError? err = null)
    {
        Ok = ok;
        Err = err;
    }
}
