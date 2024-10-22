namespace Events.Domain.Exceptions;

public class ItemNotFoundException : Exception
{
    public string ItemName { get; }

    public ItemNotFoundException(string item)
        : base($"{item} not found")
    {
        ItemName = item;
    }
}
