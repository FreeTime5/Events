using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Events.Api.Filters;

public class BindingFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors);

            var errorMessage = GetErrorMessage(errors);

            throw new ValidationException(errorMessage);
        }
    }

    private string GetErrorMessage(IEnumerable<ModelError> errors)
    {
        string message = "";
        foreach (var error in errors)
        {
            message = string.Concat(message, error.ErrorMessage);
        }
        return message;
    }
}
