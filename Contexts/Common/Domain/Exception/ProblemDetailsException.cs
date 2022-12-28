namespace Common.Domain;

public class ProblemDetailsException : Exception
{
    private readonly ProblemDetails _problemDetails = new();

    public ProblemDetails ToProblemDetails(string instance)
    {
        SetInstance(instance);
        return _problemDetails;
    }

    protected void SetTitle(string title) => _problemDetails.Title = title;

    protected void SetDetail(string detail) => _problemDetails.Detail = detail;

    protected void SetStatusCode(HttpStatusCode status) => _problemDetails.Status = (int)status;

    private void SetInstance(string instance) => _problemDetails.Instance = instance;
}