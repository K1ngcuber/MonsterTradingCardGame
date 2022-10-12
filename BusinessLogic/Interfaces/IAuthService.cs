using Models;

namespace BusinessLogic.Interfaces;

public interface IAuthService
{
    public Task RegisterAsync(User user);
    public Task LoginAsync(User user);
}