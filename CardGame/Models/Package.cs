namespace CardGame.Models;

public class Package : BaseEntity
{
    public int Price { get; set; }
    public ICollection<Card> Cards { get; set; }
}