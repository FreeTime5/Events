namespace Events.Application.Exceptions;

public class ItemAlreadyAddedException : Exception
{
    public string ItemName { get; set; }

    public ItemAlreadyAddedException(string item)
        : base($"{item} is already exist")
    {
        ItemName = item;
    }
}
