namespace Models;

public class BaseEntity : BaseEntity<Guid>
{
}

public class BaseEntity<TKey>
{
    public TKey Id { get; set; }
}

