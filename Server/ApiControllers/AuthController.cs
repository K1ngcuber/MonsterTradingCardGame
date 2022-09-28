using CustomServer.Attributes;

namespace CustomServer.ApiControllers;

[ApiController]
[Route("api/auth")]
public class AuthController
{
    [HttpPost]
    public async Task<string> Get(string body)
    {
        await Task.Delay(1000);
        return body;
    }
}