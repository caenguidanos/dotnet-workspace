namespace Common.Domain;

public class Result<S>
{
    public S Ok { get; set; } = default(S)!;

    public IError? Err { get; set; } = null;

    public dynamic Switch<H, T>(Func<S, H> onValue, Func<IError, T> onError)
        where H : notnull where T : notnull
    {
        if (Err is not null)
        {
            return onError(Err);
        }

        return onValue(Ok);
    }
}
