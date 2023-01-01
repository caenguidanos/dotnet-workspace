namespace Common.Core;

public sealed class TimeStampException : ProblemDetailsException
{
    public TimeStampException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.FailedDependency));
        SetDetail("The timestamp can't be the default");
        SetStatusCode(HttpStatusCode.FailedDependency);
    }
}