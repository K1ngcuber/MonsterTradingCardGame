namespace CardGame.Models;

public class Deck : BaseEntity
{
    public ICollection<Guid> Cards { get; set; }
}