using BusinessLogic.Interfaces;
using CustomServer.Exceptions;
using Infrastructure;
using Models;

namespace BusinessLogic.Services;

public class AuthService : IAuthService
{
    private readonly Repository<User> _repo;

    public AuthService()
    {
        _repo = new Repository<User>();
    }

    public async Task RegisterAsync(User user)
    {
        user.Id = Guid.NewGuid();
        await _repo.AddAsync(user);
    }

    public async Task LoginAsync(User user)
    {
        var check = await _repo.GetAsync("where username = @0 and password = @1", 
            new object[] {user.Username, user.Password});

        if (check is null) 
            throw new UnauthorizedException("Username or password is incorrect");

        //TODO: save login somehow
    }
}