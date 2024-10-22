namespace Events.Domain.Exceptions;

public class UserNotSignedInException : Exception
{
    public UserNotSignedInException()
        : base("User not sign in")
    {
    }
}
