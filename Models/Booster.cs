namespace Models;

public class Booster : BaseEntity
{
    public ICollection<Card> Cards { get; set; }
}