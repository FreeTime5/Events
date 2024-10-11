using Events.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Services.ServiciesErrors;

public static class ValidationErrors
{
    public static Error EmptyStringError { get; } = new Error("String can not be empty", "", "string");
}
