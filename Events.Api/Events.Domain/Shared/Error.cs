using System.Text.Json;

namespace Events.Domain.Shared;

public class Error
{
    public string Message { get; set; }


    public Error(string message)
    {
        Message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
