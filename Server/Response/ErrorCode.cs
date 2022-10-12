namespace CustomServer.Response;

public class ErrorCode : ActionResult
{
    public ErrorCode(int statusCode, object? actionResult) : base(statusCode, actionResult)
    {
    }
}