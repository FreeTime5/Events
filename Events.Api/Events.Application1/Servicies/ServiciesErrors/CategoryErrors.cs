using Events.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Services.ServiciesErrors;

public static class CategoryErrors
{
    public static Error CategoryNotFound { get; } = new Error("Category not found", "", "Category");
}
