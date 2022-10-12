namespace CustomServer.Response;

public class OkResponse : ActionResult
{
    public OkResponse(object? result) : base(200,result)
    {
    }
}