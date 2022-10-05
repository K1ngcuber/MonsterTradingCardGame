using CustomServer.Attributes;
using CustomServer.Attributes.ParameterAttributes;
using CustomServer.Attributes.RouteAttributes;
using Models;

namespace CardGame.ApiControllers;

[ApiController]
[Route("api/user")]
public class AuthController : Controller
{
    
    [HttpPost]
    public async Task<int> Register([FromBody] User user)
    {
        Console.WriteLine("User: " + user.Username);
        await Task.CompletedTask;

        return 200;
    }
    
    
}