using System.Text.Json;

namespace Events.Domain.Shared;

public class Error
{
    public string Message { get; set; }

    public int Code { get; set; }

    public Error(string message, int code)
    {
        Message = message;
        Code = code;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
