using CustomServer.Attributes;

namespace CustomServer.ApiControllers;

[ApiController]
[Route("api/test")]
public class TestController
{
    [HttpGet]
    public string Test()
    {
        return "Hello World GET";
    }
    
    [HttpPost]
    public string TestPost()
    {
        return "Hello World POST";
    }
}