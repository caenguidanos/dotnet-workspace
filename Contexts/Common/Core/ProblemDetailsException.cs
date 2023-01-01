namespace Common.Core;

public class ProblemDetailsException : Exception
{
    private readonly ProblemDetails _problemDetails = new();

    public ProblemDetails ToProblemDetails(in string instance)
    {
        SetInstance(instance);

        return _problemDetails;
    }

    protected void SetTitle(in string title) => _problemDetails.Title = title;

    protected void SetDetail(in string detail) => _problemDetails.Detail = detail;

    protected void SetStatusCode(in HttpStatusCode status) => _problemDetails.Status = (int)status;

    private void SetInstance(in string instance) => _problemDetails.Instance = instance;
}