using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using CustomServer.Attributes;
using CustomServer.Attributes.RouteAttributes;
using CustomServer.Response;

namespace CardGame.ApiControllers;

[ApiController]
[Route("api/card")]
public class CardController : Controller
{
    private readonly ICardService _service;

    public CardController() => _service = new CardService();

    [HttpGet]
    public async Task<ActionResult> GetCardsByUserId()
    {
        //TODO: username from header
        var cards = await _service.ShowAllCardsByUserAsync("Test1234");

        return Ok(cards);
    }
}