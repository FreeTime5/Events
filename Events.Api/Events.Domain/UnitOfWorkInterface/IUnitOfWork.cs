using Events.Domain.RepositoryInterfaces;

namespace Events.Domain.UnitOfWorkInterface;

/// <summary>
/// Interface for unit of work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Get repository for work with categories.
    /// </summary>
    ICategoryRepository CategoryRepository { get; }

    /// <summary>
    /// Set category repository for current unit of work.
    /// </summary>
    void SetCategoryRepository();

    /// <summary>
    /// Get repository for work with events.
    /// </summary>
    IEventRepsository EventRepsository { get; }

    /// <summary>
    /// Set event repository for current unit of work.
    /// </summary>
    void SetEventRepository();

    /// <summary>
    /// Get repository for work with registrations.
    /// </summary>
    IRegistrationRepository RegistrationRepository { get; }

    /// <summary>
    /// Set registration repository for current unit of work.
    /// </summary>
    void SetRegistrationRepository();

    /// <summary>
    /// Save all chenges to database.
    /// </summary>
    /// <returns></returns>
    Task Save();
}

