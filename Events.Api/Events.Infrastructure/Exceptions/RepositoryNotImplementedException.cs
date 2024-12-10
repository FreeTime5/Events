namespace Events.Infrastructure.Exceptions;

public class RepositoryNotImplementedException : Exception
{
    public string RepositoryName { get; }

    public RepositoryNotImplementedException(string repositoryName) : base($"{repositoryName} was not implemented")
    {
        RepositoryName = repositoryName;
    }
}
