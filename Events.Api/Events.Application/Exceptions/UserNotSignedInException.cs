
namespace Events.Application.Exceptions;

public class UserNotSignedInException : Exception
{
    public UserNotSignedInException()
        : base("User not sign in")
    {
    }
}
