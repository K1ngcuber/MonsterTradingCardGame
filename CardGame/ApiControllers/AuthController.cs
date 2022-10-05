using CustomServer.Attributes;
using CustomServer.Attributes.ParameterAttributes;
using CustomServer.Attributes.RouteAttributes;
using Models;

namespace CardGame.ApiControllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    [HttpPost("login")]
    public async Task<object> Get([FromRoute] int id)
    {
        await Task.Delay(1000);
        return "Geht" + id;
    }
    
    
}