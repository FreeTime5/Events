using Events.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Services.ServiciesErrors;

public static partial class EventErrors
{
    public static Error EventNotFound { get; } = new Error("Event not found with such id", "", "Event");

    public static Error DuplicatedEventTitle { get; } = new Error("There are event with such title", "", "Event");
}
