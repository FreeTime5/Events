using Events.Domain.Exceptions;
using Events.Domain.Shared;
using FluentValidation;
using System.Net;

namespace Events.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ItemNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ItemAlreadyAddedException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.Conflict, ex.Message);
            }
            catch (UserHaveNoPermissionException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.UnavailableForLegalReasons, ex.Message);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.UnprocessableEntity, ex.Message);
            }
            catch (UserNotSignedInException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest, ex.Message);
            }

        }

        private async Task HandleExceptionAsync(HttpContext context, string logMessage, HttpStatusCode statusCode, string message)
        {
            logger.LogError(logMessage);

            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;

            var error = new Error(message, (int)statusCode);

            var result = error.ToString();

            await response.WriteAsJsonAsync(result);
        }

    }
}
