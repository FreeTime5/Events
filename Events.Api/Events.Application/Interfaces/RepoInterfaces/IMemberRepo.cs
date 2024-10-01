using Events.Application.Models;
using Events.Application.Servicies.MemberService.DTOs;
using Events.Domain.Entities;

namespace Events.Application.Interfaces;

public interface IMemberRepo
{
    Task<List<User>> GetAllFromEvent(Guid eventId);

    Task<Result> AddToEvent(string memberId, Guid eventId);

    Task<Result> RemoveFromEvent(string memberId, Guid evId);

    Task<User?> GetById(Guid id);
}
