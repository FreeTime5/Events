namespace Events.Application.Exceptions;

public class UserHaveNoPermissionException : Exception
{
    public UserHaveNoPermissionException()
       : base("User do not have permission")
    {
    }
    public UserHaveNoPermissionException(string message)
        : base(message)
    {
    }
}
