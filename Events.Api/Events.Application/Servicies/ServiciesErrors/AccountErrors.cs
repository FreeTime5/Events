using Events.Domain.Shared;

namespace Events.Application.Servicies.ServiciesErrors;

public static class AccountErrors
{
    public static Error UserNotFound { get; } = new Error("User not found", "", "User");

    public static Error IncorrectPassword { get; } = new Error("Password is incorrect for user", "", "User");
}
