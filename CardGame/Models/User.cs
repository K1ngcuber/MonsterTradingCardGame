namespace CardGame.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }

    public int Coins { get; set; }
    public Deck Deck { get; set; }
    
    public ICollection<Guid> Cards { get; set; }
}