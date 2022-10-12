namespace CustomServer.Response;

public abstract class ActionResult
{
    public int StatusCode;
    public object? Value;

    protected ActionResult (int statusCode, object? actionResult)
    {
        StatusCode = statusCode;
        Value = actionResult;
    }
}