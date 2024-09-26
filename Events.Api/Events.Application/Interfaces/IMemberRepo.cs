using Events.Application.Models;
using Events.Domain.Entities;

namespace Events.Application.Interfaces;

public interface IMemberRepo
{
    Task<List<User>> GetAllFromEvent(Event ev);

    Task<Result> AddToEvent(string memberId, Guid eventId);

    Task<Result> RemoveFromEvent(string memberId, Guid evId);

    Task<User?> GetById(Guid id);
}
