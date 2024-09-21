using Events.Domain.Shared;

namespace Events.Application.Models;

public class Result
{
    private Result(bool secceeded, IEnumerable<Error> errors)
    {
        Secceeded = secceeded;
        Errors = errors.ToArray();
    }

    public bool Secceeded { get; init; }

    public Error[] Errors { get; init; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<Error>());
    }

    public static Result Failure(IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }
}
