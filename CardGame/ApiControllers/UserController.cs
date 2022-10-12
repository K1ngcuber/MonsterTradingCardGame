using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using CustomServer.Attributes;
using CustomServer.Attributes.ParameterAttributes;
using CustomServer.Attributes.RouteAttributes;
using CustomServer.Response;
using Models;

namespace CardGame.ApiControllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IAuthService _authService;

    public UserController() => _authService = new AuthService();

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] User user)
    {
        try
        {
            await _authService.RegisterAsync(user);
            await Task.CompletedTask;

            return Ok("Registered successfully");
        }
        catch
        {
            return InternalServerError();
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] User user)
    {
        await _authService.LoginAsync(user);

        return Ok("Logged in");
    }
}