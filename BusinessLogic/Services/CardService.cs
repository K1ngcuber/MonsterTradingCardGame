using BusinessLogic.Interfaces;
using CustomServer.Exceptions;
using Infrastructure;
using Models;

namespace BusinessLogic.Services;

public class CardService : ICardService
{
    private readonly Repository<Card> _repo;
    private readonly Repository<User> _userRepo;

    public CardService()
    {
        _userRepo = new Repository<User>();
        _repo = new Repository<Card>();
    }

    public async Task<List<Card>> ShowAllCardsByUserAsync(string userName)
    {
        var user = await _userRepo.GetAsync("where UserName = @0", new object[] {userName});
        if(user is null) throw new NotFoundException("User not found");
        return await _repo.FindAsync("where UserId = @0", new object[] {user.Id});
    }
}