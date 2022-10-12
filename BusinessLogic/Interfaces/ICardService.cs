using Models;

namespace BusinessLogic.Interfaces;

public interface ICardService
{
    Task<List<Card>> ShowAllCardsByUserAsync(string userName);
}