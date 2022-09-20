namespace CardGame.Models;

public class Card : BaseEntity
{
    public string Name { get; set; }
    public int Damage { get; }
    public ElementType ElementType { get; set; }
}

public enum ElementType
{
    Water,
    Fire,
    Normal
}