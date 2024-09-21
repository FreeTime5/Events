using Events.Application.Models;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface IMemberRepo
{
    Task<List<Member>> GetAllFromEvent(Event ev);

    Task<Result> AddToEvent(Guid memberId, Guid eventId);

    Task<Result> RemoveFromEvent(Guid memberId, Guid evId);

    Task<Member> GetById(Guid id);
}
