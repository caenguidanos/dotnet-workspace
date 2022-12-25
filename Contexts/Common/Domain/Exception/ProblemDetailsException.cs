namespace Common.Domain;

public class ProblemDetailsException : Exception
{
    private readonly ProblemDetails _problemDetails = new();

    public void AsProblemDetails(out ProblemDetails problemDetails)
    {
        problemDetails = _problemDetails;
    }

    protected void SetTitle(string title) => _problemDetails.Title = title;

    protected void SetDetail(string detail) => _problemDetails.Detail = detail;

    protected void SetStatusCode(HttpStatusCode status) => _problemDetails.Status = (int)status;

    public void SetInstance(string instance) => _problemDetails.Instance = instance;
}