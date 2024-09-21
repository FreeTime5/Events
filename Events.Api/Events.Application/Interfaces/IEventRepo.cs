using Events.Application.Models;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface IEventRepo
{
    Task<List<Event>> GetAll();

    Task<Event> GetById(Guid id);

    Task<Event> GetByName(string name);

    Task<Result> Add(Event ev);

    Task<Result> Update(Event ev);

    Task<Result> Delete(Guid eventId);
}
