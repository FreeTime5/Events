using Events.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Security.Claims;

namespace Events.Api.Authorization.Handlers
{
    public class RolesHandler : AuthorizationHandler<RoleRequirement>
    {
        public RolesHandler()
        {
            
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }

            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;

            if (requirement.Roles.Contains(role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
