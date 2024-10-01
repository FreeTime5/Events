using Events.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.DatabaseErrors;

public static class CategoryErrors
{
    public static Error DuplicatedCategory { get; } = new Error("This category is already exists", "", "Category");

    public static Error CategoryNotFound { get; } = new Error("Category not found", "", "Category");
}
