using CustomServer.Response;

namespace CardGame.ApiControllers;

public class Controller
{
    protected static ActionResult Ok(object? value = null)
    {
        return new OkResponse(value);
    }

    protected static ActionResult InternalServerError(object? value = null)
    {
        return new ErrorCode(500,value);
    }
}