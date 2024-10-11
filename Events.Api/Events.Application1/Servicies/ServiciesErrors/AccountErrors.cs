using Events.Domain.Shared;

namespace Events.Application.Services.ServiciesErrors;

public static class AccountErrors
{
    public static Error UserNotFound { get; } = new Error("User not found", "", "User");

    public static Error IncorrectPassword { get; } = new Error("Password is incorrect for user", "", "User");

    public static Error UserNotSignedIn { get; } = new Error("User must be signed in", "", "User");

    public static Error UserHaveNotPermision { get; } = new Error("User have not permission for action", "", "User");
}
