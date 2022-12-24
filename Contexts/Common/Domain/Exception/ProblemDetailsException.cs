namespace Common.Domain;

public class ProblemDetailsException : Exception
{
    private readonly ProblemDetails _problemDetails = new();

    public ProblemDetails GetProblemDetails(string instance)
    {
        _problemDetails.Instance = instance;
        return _problemDetails;
    }

    protected void SetStatusCode(HttpStatusCode status) => _problemDetails.Status = (int)status;

    protected void SetTitle(string title) => _problemDetails.Title = title;

    protected void SetDetail(string detail) => _problemDetails.Detail = detail;
}